using Reqnroll;
using EnseK_API_Automation;
using EnseK_API_Automation.config;
using EnseK_API_Automation.Helpers;
using EnseK_API_Automation.Models;
using EnseK_API_Automation.Models.Response;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;

namespace ENSEK_Test.Steps
{
    [Binding]
    public class GetOrdersSteps
    {
        private readonly ScenarioContext scenarioContext;
        private APIClient apiClient;
        private List<ExpectedOrder> placedOrders;
        private List<Order> retrievedOrders;
        private List<string> validationErrors = new();

        public GetOrdersSteps(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
            apiClient = new APIClient(useAuthentication: true);
        }

        // ===== BUY FUEL AND VALIDATE =====

        [When("the user buys all available fuels")]
        public async Task WhenTheUserBuysAllAvailableFuels()
        {
            var response = await apiClient.GetEnergyData();
            Assert.That((int)response.StatusCode, Is.EqualTo(200), "❌ Failed to fetch inventory");

            var inventory = JsonConvert.DeserializeObject<EnergyInventory>(response.Content);
            Assert.IsNotNull(inventory, "❌ Inventory deserialization failed");

            placedOrders = await OrderHelper.BuyAvailableFuels(apiClient, inventory, 1, validationErrors);
        }

        [Then("the placed orders should be found in the order list")]
        public async Task ThenThePlacedOrdersShouldBeFoundInTheOrderList()
        {
            var ordersResponse = await apiClient.GetOrders();
            Assert.That((int)ordersResponse.StatusCode, Is.EqualTo(200), "❌ Failed to fetch orders");

            var actualOrders = JsonConvert.DeserializeObject<List<Order>>(ordersResponse.Content);
            Assert.IsNotNull(actualOrders, "❌ Orders deserialization failed");

            OrderHelper.ValidatePlacedOrders(placedOrders, actualOrders, validationErrors);

            if (validationErrors.Any())
            {
                Assert.Fail("❌ Validation errors:\n" + string.Join("\n", validationErrors));
            }
        }

        // ===== COUNT ORDERS BEFORE TODAY =====

        [When("the user retrieves the list of All orders")]
        public async Task WhenTheUserRetrievesTheListOfOrders()
        {
            var response = await apiClient.GetOrders();
            Assert.That((int)response.StatusCode, Is.EqualTo(200), "❌ Failed to fetch orders");

            retrievedOrders = JsonConvert.DeserializeObject<List<Order>>(response.Content);
            Assert.IsNotNull(retrievedOrders, "❌ Orders deserialization failed");
        }

        [Then("the number of orders created before today should be shown")]
        public void ThenTheNumberOfOrdersCreatedBeforeTodayShouldBeShown()
        {
            int pastOrderCount = OrderHelper.CountOrdersBeforeToday(retrievedOrders);
            TestContext.WriteLine($"✅ Orders before today: {pastOrderCount}");
        }

        // ===== CLEANUP =====

        [AfterScenario]
        public void AfterScenario()
        {
            apiClient?.Dispose();
        }
    }
}
