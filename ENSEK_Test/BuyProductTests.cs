using EnseK_API_Automation;
using EnseK_API_Automation.config;
using EnseK_API_Automation.constants;
using EnseK_API_Automation.Models.Response;
using EnseK_API_Automation.Helpers;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ENSEK_Test
{
    public class BuyProductTests
    {
        private APIClient apiClient;

        [SetUp]
        public async Task Setup()
        {
            // Set environment and initialize API client
            EnvironmentConfig.CurrentEnvironment = Environment.GetEnvironmentVariable("ENSEK_ENV") ?? "QA";
            ApiCredentials.ClearOverrides();
            apiClient = new APIClient(useAuthentication: true);

            // Reset system state before each test
            var response = await apiClient.Reset();
            Assert.That((int)response.StatusCode, Is.EqualTo(200), "Reset failed.");
        }
        [TearDown]
        public void AfterTest()
        {
            apiClient.Dispose();
        }

        [Test]
        public async Task VerifyUserCanBuyProducts()
        {
            var response = await apiClient.BuyProduct("1", "2");

            Assert.That((int)response.StatusCode, Is.EqualTo(200));
            CommonHelper.LogAndValidateResponse<dynamic>(response);
        }

        [Test]
        public async Task VerifyUnauthorizedUserCannotBuyProduct()
        {
            ApiCredentials.OverrideUsername = "special_user";
            ApiCredentials.OverridePassword = "special_password";

            var response = await apiClient.BuyProduct("1", "2");

            Assert.That((int)response.StatusCode, Is.EqualTo(401));
            CommonHelper.LogAndValidateResponse<dynamic>(response);
        }

        [Test]
        public async Task VerifyUserCanBuyProductWithoutAuthentication()
        {
            var unauthClient = new APIClient(useAuthentication: false);
            var response = await unauthClient.BuyProduct("1", "2");

            Assert.That((int)response.StatusCode, Is.EqualTo(200));
            CommonHelper.LogAndValidateResponse<dynamic>(response);
        }

        [Test]
        public async Task VerifyBadRequestsAreHandled()
        {
            var response = await apiClient.BuyProduct("1124422324242424", "242424242424");

            Assert.That((int)response.StatusCode, Is.EqualTo(400));
            CommonHelper.LogAndValidateResponse<dynamic>(response);
        }

        [Test]
        public async Task Verify_BuyEachAvailableFuel_ShouldValidateDetails_On_TheResponse_Message()
        {
            var client = new APIClient();
            var response = await client.GetEnergyData();

            Assert.That((int)response.StatusCode, Is.EqualTo(200), "Failed to fetch energy inventory.");

            var inventory = JsonConvert.DeserializeObject<EnergyInventory>(response.Content);
            Assert.IsNotNull(inventory, "Failed to deserialize energy inventory.");

            var fuels = new Dictionary<string, EnergySource>
            {
                ["electric"] = inventory.Electric,
                ["gas"] = inventory.Gas,
                ["nuclear"] = inventory.Nuclear,
                ["oil"] = inventory.Oil
            };

            var errors = new List<string>();
            int buyingQty = 1;

            foreach (var (fuelName, source) in fuels)
            {
                await OrderHelper.ValidateFuelPurchaseDetails(fuelName, source, buyingQty, errors);
            }

            if (errors.Any())
            {
                Assert.Fail("❌ One or more assertions failed:\n" + string.Join("\n", errors));
            }
        }

       
    }
}
