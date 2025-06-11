using EnseK_API_Automation;
using EnseK_API_Automation.config;
using EnseK_API_Automation.Helpers;
using EnseK_API_Automation.Models.Response;
using Newtonsoft.Json;
using Reqnroll;
using RestSharp;

namespace ENSEK_Test.steps
{
    [Binding]
    public class BuyProductSteps
    {
        private APIClient apiClient;
        private RestResponse response;
        private RestResponse fuelResponse;
        private int expectedStatusCode;
        private List<string> validationErrors = new();

        //[Given("the system is reset to a clean state")]
        //public async Task GivenTheSystemIsResetToACleanState()
        //{
        //    EnvironmentConfig.CurrentEnvironment = Environment.GetEnvironmentVariable("ENSEK_ENV") ?? "QA";
        //    ApiCredentials.ClearOverrides();
        //    apiClient = new APIClient(useAuthentication: true);
        //    var resetResponse = await apiClient.Reset();
        //    Assert.That((int)resetResponse.StatusCode, Is.EqualTo(200), "Reset failed.");
        //}

        [Given(@"the user's credentials are overridden with ""(.*)"" and ""(.*)""")]
        public void GivenUserCredentialsAreOverridden(string username, string password)
        {
            ApiCredentials.OverrideUsername = username;
            ApiCredentials.OverridePassword = password;
        }

        [When(@"the user buys product with ID ""(.*)"" and quantity ""(.*)""")]
        public async Task WhenTheUserBuysProduct(string productId, string quantity)
        {
            apiClient = new APIClient(useAuthentication: true);
            response = await apiClient.BuyProduct(productId, quantity);
        }

        [When(@"an unauthenticated user buys product with ID ""(.*)"" and quantity ""(.*)""")]
        public async Task WhenAnUnauthenticatedUserBuysProduct(string productId, string quantity)
        {
            var unauthClient = new APIClient(useAuthentication: false);
            response = await unauthClient.BuyProduct(productId, quantity);
        }

        [Then(@"the response code should be (\d+)")]
        public void ThenTheResponseCodeShouldBe(int statusCode)
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(statusCode));
            CommonHelper.LogAndValidateResponse<dynamic>(response);
        }

        [When(@"the user fetches available fuels and buys each with quantity ""(.*)""")]
        public async Task WhenTheUserFetchesAndBuysEachFuel(string quantityStr)
        {
            int quantity = int.Parse(quantityStr);
            var client = new APIClient(useAuthentication: true);
            fuelResponse = await client.GetEnergyData();
            Assert.That((int)fuelResponse.StatusCode, Is.EqualTo(200), "Failed to fetch energy inventory.");

            var inventory = JsonConvert.DeserializeObject<EnergyInventory>(fuelResponse.Content);
            Assert.IsNotNull(inventory, "Failed to deserialize energy inventory.");

            var fuels = new Dictionary<string, EnergySource>
            {
                ["electric"] = inventory.Electric,
                ["gas"] = inventory.Gas,
                ["nuclear"] = inventory.Nuclear,
                ["oil"] = inventory.Oil
            };

            foreach (var (fuelName, source) in fuels)
            {
                await OrderHelper.ValidateFuelPurchaseDetails(fuelName, source, quantity, validationErrors);
            }
        }

        [Then(@"all purchase responses should be valid")]
        public void ThenAllPurchaseResponsesShouldBeValid()
        {
            if (validationErrors.Any())
            {
                Assert.Fail("❌ One or more assertions failed:\n" + string.Join("\n", validationErrors));
            }
        }
    }
}
