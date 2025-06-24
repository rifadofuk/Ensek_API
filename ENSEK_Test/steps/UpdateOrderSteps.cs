using EnseK_API_Automation;
using EnseK_API_Automation.Models.Request;
using EnseK_API_Automation.Models.Response;
using Newtonsoft.Json;
using Reqnroll;

namespace ENSEK_Test.Steps
{
    [Binding]
    public class UpdateOrderSteps
    {
        private APIClient client;
        private List<Order> orders;
        private EnergyInventory inventory;
        private Order originalOrder;
        private Order updatedOrder;
        private OrderResource updatePayload;


        [Given("the user has existing orders and inventory")]
        public async Task GivenTheUserHasExistingOrdersAndInventory()
        {
            client = new APIClient(useAuthentication: true);
            var ordersResponse = await client.GetOrders();
            Assert.That((int)ordersResponse.StatusCode, Is.EqualTo(200), "❌ Failed to fetch orders");

            orders = JsonConvert.DeserializeObject<List<Order>>(ordersResponse.Content);
            Assert.IsNotNull(orders, "❌ Failed to deserialize orders");
            Assert.IsNotEmpty(orders, "❌ No orders available to update");

            var inventoryResponse = await client.GetEnergyData();
            Assert.That((int)inventoryResponse.StatusCode, Is.EqualTo(200), "❌ Failed to fetch inventory");

            inventory = JsonConvert.DeserializeObject<EnergyInventory>(inventoryResponse.Content);
            Assert.IsNotNull(inventory, "❌ Failed to deserialize energy inventory");
        }

        [When(@"the user updates the first order with quantity (.*)")]
        public async Task WhenTheUserUpdatesTheFirstOrderWithQuantity(int newQuantity)
        {
            originalOrder = orders.First();

            int energyId = originalOrder.Fuel.ToLower() switch
            {
                "electric" => inventory.Electric.Energy_Id,
                "gas" => inventory.Gas.Energy_Id,
                "nuclear" => inventory.Nuclear.Energy_Id,
                "oil" => inventory.Oil.Energy_Id,
                _ => throw new Exception($"❌ Unknown fuel type: {originalOrder.Fuel}")
            };

            updatePayload = new OrderResource
            {
                id = originalOrder.Id,
                quantity = newQuantity,
                energy_id = energyId
            };

            var updateResponse = await client.UpdateOrder(updatePayload, originalOrder.Id);
            Assert.That((int)updateResponse.StatusCode, Is.EqualTo(200), "❌ Order update failed");

            updatedOrder = JsonConvert.DeserializeObject<Order>(updateResponse.Content);
            Assert.IsNotNull(updatedOrder, "❌ Failed to deserialize updated order");
        }

        [Then(@"the order should be updated successfully with the quantity (.*)")]
        public void ThenTheOrderShouldBeUpdatedSuccessfully(int expectedQuantity)
        {
            Assert.That(updatedOrder.Id, Is.EqualTo(originalOrder.Id), "❌ Order ID mismatch");
            Assert.That(updatedOrder.Quantity, Is.EqualTo(expectedQuantity), "❌ Quantity was not updated");
            Assert.That(updatedOrder.Fuel, Is.EqualTo(originalOrder.Fuel), "❌ Fuel type mismatch");

            TestContext.WriteLine($"✅ Order {updatedOrder.Id} updated: quantity = {updatedOrder.Quantity}, energy_id = {updatePayload.energy_id}");
        }

        //[When("the user updates the following orders:")]
        //public void WhenTheUserUpdatesTheFollowingOrders(DataTable dataTable)
        //{
        //    foreach (var row in dataTable.Rows)
        //    {
        //        string orderId = row["OrderID"];
        //        int qnty = int.Parse(row["kerer"]);
        //        int energyID = int.Parse(row["id"]);

        //        var payload = new Order
        //        {
        //            Id = orderId,
        //            Quantity = qnty,
        //        };

        //    }
        //}

        //[Then("all orders should be updated successfully")]
        //public void ThenAllOrdersShouldBeUpdatedSuccessfully()
        //{
        //    throw new PendingStepException();
        //}


        [AfterScenario]
        public void AfterScenario()
        {
            client?.Dispose();
        }
    }
}
