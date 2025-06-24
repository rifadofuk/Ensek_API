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
        private readonly IApiClientFactory _clientFactory;
        private APIClient _apiClient;
        private RestResponse response;
        private RestResponse fuelResponse;
        private readonly FeatureContext _featureContext;
        private readonly List<string> validationErrors = new();

        public BuyProductSteps(FeatureContext featureContext, IApiClientFactory clientFactory)
        {
            _featureContext = featureContext;
            _clientFactory = clientFactory;
        }

        [Given(@"the user's credentials are overridden with ""(.*)"" and ""(.*)""")]
        public void GivenUserCredentialsAreOverridden(string username, string password)
        {
            Console.WriteLine(_featureContext.FeatureInfo.Title);
            ApiCredentials.OverrideUsername = username;
            ApiCredentials.OverridePassword = password;
        }

        [When(@"the user buys product with ID ""(.*)"" and quantity ""(.*)""")]
        public async Task WhenTheUserBuysProduct(string productId, string quantity)
        {
            _apiClient = _clientFactory.Create(useAuthentication: true);
            response = await _apiClient.BuyProduct(productId, quantity);
        }

        [When(@"an unauthenticated user buys product with ID ""(.*)"" and quantity ""(.*)""")]
        public async Task WhenAnUnauthenticatedUserBuysProduct(string productId, string quantity)
        {
            _apiClient = _clientFactory.Create(useAuthentication: false);
            response = await _apiClient.BuyProduct(productId, quantity);
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
            var client = _clientFactory.Create(useAuthentication: true);
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
