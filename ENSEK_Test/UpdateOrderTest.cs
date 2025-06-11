using EnseK_API_Automation;
using EnseK_API_Automation.Models.Request;
using EnseK_API_Automation.Models.Response;
using Newtonsoft.Json;

namespace ENSEK_Test;

public class UpdateOrderTest
{
    private APIClient client;
    private List<Order> orders;
    private EnergyInventory inventory;

    [SetUp]
    public async Task Setup()
    {
        // Initialize API client with authentication
        client = new APIClient(useAuthentication: true);

        // Reset API state before test
        var reset_esponse = await client.Reset();
        Assert.That((int)reset_esponse.StatusCode, Is.EqualTo(200), "❌ Reset Failed");

        // Fetch existing orders
        var ordersResponse = await client.GetOrders();
        Assert.That((int)ordersResponse.StatusCode, Is.EqualTo(200), "❌ Failed to fetch orders");

        // Deserialize orders response
        orders = JsonConvert.DeserializeObject<List<Order>>(ordersResponse.Content);
        Assert.IsNotNull(orders, "❌ Could not deserialize orders.");
        Assert.IsNotEmpty(orders, "❌ No orders available to update.");

        // Fetch current energy inventory for dynamic energy IDs
        var inventoryResponse = await client.GetEnergyData();
        Assert.That((int)inventoryResponse.StatusCode, Is.EqualTo(200), "❌ Failed to fetch energy inventory");

        // Deserialize energy inventory response
        inventory = JsonConvert.DeserializeObject<EnergyInventory>(inventoryResponse.Content);
        Assert.IsNotNull(inventory, "❌ Could not deserialize energy inventory");
    }

    [Test]
    public async Task VerifyUserCanUpdateOrder()
    {
        // Select first order and prepare updated quantity
        var orderToUpdate = orders.First();
        int newQuantity = orderToUpdate.Quantity + 1;

        // Determine correct energy_id based on order's fuel type
        int energy = orderToUpdate.Fuel.ToLower() switch
        {
            "electricf" => inventory.Electric.Energy_Id,
            "gas" => inventory.Gas.Energy_Id,
            "nuclear" => inventory.Nuclear.Energy_Id,
            "oil" => inventory.Oil.Energy_Id,
            _ => throw new Exception($"❌ Unknown fuel type: {orderToUpdate.Fuel}")
        };

        // Create update payload with new quantity and energy_id
        var updatePayload = new OrderResource
        {
            id = orderToUpdate.Id,
            quantity = newQuantity,
            energy_id = energy
        };

        // Send update request
        var updateResponse = await client.UpdateOrder(updatePayload, orderToUpdate.Id);
        Assert.That((int)updateResponse.StatusCode, Is.EqualTo(200), "❌ Order update failed.");

        // Deserialize and verify updated order response
        var updatedOrder = JsonConvert.DeserializeObject<Order>(updateResponse.Content);
        Assert.IsNotNull(updatedOrder, "❌ Failed to deserialize updated order.");
        Assert.That(updatedOrder.Id, Is.EqualTo(orderToUpdate.Id), "❌ Order ID mismatch.");
        Assert.That(updatedOrder.Quantity, Is.EqualTo(newQuantity), "❌ Quantity was not updated.");
        Assert.That(updatedOrder.Fuel, Is.EqualTo(orderToUpdate.Fuel), "❌ Fuel type mismatch.");

        // Log success message
        TestContext.WriteLine($"✅ Order {updatedOrder.Id} updated: quantity changed to {updatedOrder.Quantity}, energy_id={updatePayload.energy_id}");
    }
    [TearDown]
    public void AfterTest()
    {
        client.Dispose();
    }

}
