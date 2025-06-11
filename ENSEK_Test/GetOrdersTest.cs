using EnseK_API_Automation;
using EnseK_API_Automation.config;
using EnseK_API_Automation.Models;
using EnseK_API_Automation.Models.Response;
using EnseK_API_Automation.Helpers;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ENSEK_Test
{
    public class GetOrdersTests
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
        public async Task BuyFuelsAndVerifyOrders_ShouldMatchPlacedOrders()
        {
            var client = new APIClient(useAuthentication: true);
            var response = await client.GetEnergyData();
            Assert.That((int)response.StatusCode, Is.EqualTo(200), "Failed to fetch inventory");

            var inventory = JsonConvert.DeserializeObject<EnergyInventory>(response.Content);
            Assert.IsNotNull(inventory);

            var errors = new List<string>();
            var placedOrders = await OrderHelper.BuyAvailableFuels(client, inventory, 1, errors);

            var ordersResponse = await client.GetOrders();
            Assert.That((int)ordersResponse.StatusCode, Is.EqualTo(200), "Failed to fetch orders");

            var actualOrders = JsonConvert.DeserializeObject<List<Order>>(ordersResponse.Content);
            Assert.IsNotNull(actualOrders, "Orders response deserialization failed.");

            OrderHelper.ValidatePlacedOrders(placedOrders, actualOrders, errors);

            if (errors.Any())
            {
                Assert.Fail("One or more validation errors:\n" + string.Join("\n", errors));
            }
        }

        [Test]
        public async Task Verify_Num_orders_were_created_before_the_current_date()
        {
            var client = new APIClient(useAuthentication: true);
            var ordersResponse = await client.GetOrders();
            Assert.That((int)ordersResponse.StatusCode, Is.EqualTo(200), "Failed to fetch orders");

            var actualOrders = JsonConvert.DeserializeObject<List<Order>>(ordersResponse.Content);
            Assert.IsNotNull(actualOrders, "Orders response deserialization failed.");

            int pastOrderCount = OrderHelper.CountOrdersBeforeToday(actualOrders);
            TestContext.WriteLine($"✅ Number of orders created before today: {pastOrderCount}");
        }
    }
}
