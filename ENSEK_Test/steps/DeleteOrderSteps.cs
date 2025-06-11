using EnseK_API_Automation;
using EnseK_API_Automation.Helpers;
using EnseK_API_Automation.Models.Response;
using Newtonsoft.Json;
using Reqnroll;
using RestSharp;
namespace ENSEK_Test.steps
{

    [Binding]
    public class DeleteOrderSteps
    {
        private APIClient apiClient;
        private List<Order> orderList;
        private RestResponse deleteResponse;

        //[Given(@"the test environment is set to ""(.*)""")]
        //public void GivenTheTestEnvironmentIsSetTo(string environment)
        //{
        //    EnvironmentConfig.CurrentEnvironment = environment;
        //    ApiCredentials.ClearOverrides();
        //    apiClient = new APIClient(useAuthentication: true);
        //}

        //[Given("the system is reset to a clean state")]
        //public async Task GivenTheSystemIsResetToACleanState()
        //{
        //    var response = await apiClient.Reset();
        //    Assert.That((int)response.StatusCode, Is.EqualTo(200), "Reset failed.");
        //}

        [When("the user retrieves the list of orders")]
        public async Task WhenTheUserRetrievesTheListOfOrders()
        {
            apiClient = new APIClient(useAuthentication: true);
            var response = await apiClient.GetOrders();
            Assert.That((int)response.StatusCode, Is.EqualTo(200), "❌ Failed to fetch orders");

            orderList = JsonConvert.DeserializeObject<List<Order>>(response.Content);
            Assert.IsNotNull(orderList, "❌ Failed to deserialize order list.");
            Assert.IsNotEmpty(orderList, "❌ No orders found to delete.");
        }

        [When("the user deletes the first order")]
        public async Task WhenTheUserDeletesTheFirstOrder()
        {
            var orderIdToDelete = orderList.First().Id;
            deleteResponse = await apiClient.DeleteOrder(orderIdToDelete);
        }

        [Then("the order deletion should return status code 200")]
        public void ThenTheOrderDeletionShouldReturnStatusCode200()
        {
            Assert.That((int)deleteResponse.StatusCode, Is.EqualTo(200), "❌ Failed to delete the order.");
            CommonHelper.LogAndValidateResponse<dynamic>(deleteResponse);
        }

        [AfterScenario]
        public void Cleanup()
        {
            apiClient?.Dispose();
        }
    }

}
