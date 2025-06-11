using EnseK_API_Automation;
using EnseK_API_Automation.config;
using EnseK_API_Automation.Helpers;
using EnseK_API_Automation.Models.Response;
using Newtonsoft.Json;
using RestSharp;

namespace ENSEK_Test;

public class DeleteOrderTest
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
    public async Task VerifyUserCanDeleteValidOrders()
    {
        // Step 1: Fetch all current orders
        var client = new APIClient(useAuthentication: true);
        var ordersResponse = await client.GetOrders();

        Assert.That((int)ordersResponse.StatusCode, Is.EqualTo(200), "❌ Failed to fetch orders");

        var orders = JsonConvert.DeserializeObject<List<Order>>(ordersResponse.Content);
        Assert.IsNotNull(orders, "❌ Failed to deserialize order list.");
        Assert.IsNotEmpty(orders, "❌ No orders found to delete.");

        // Step 2: Get the first order ID to delete
        var orderIdToDelete = orders.First().Id;

        // Step 3: Call Delete API using the order ID
        var deleteResponse = await client.DeleteOrder(orderIdToDelete); // <-- Assumes DeleteOrder(string id) exists

        // Step 4: Validate deletion success
        CommonHelper.LogAndValidateResponse<dynamic>(deleteResponse);
        Assert.That((int)deleteResponse.StatusCode, Is.EqualTo(200), "❌ Failed to delete the order.");
    }


}
