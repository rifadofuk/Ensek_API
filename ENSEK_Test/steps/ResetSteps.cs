using EnseK_API_Automation;
using EnseK_API_Automation.config;
using EnseK_API_Automation.Helpers;
using Reqnroll;
using RestSharp;

namespace ENSEK_Test.Steps
{
    [Binding]
    public class ResetSteps
    {
        private APIClient apiClient;
        private RestResponse response;

        [When("the user performs a reset with valid credentials")]
        public async Task WhenTheUserPerformsResetWithValidCredentials()
        {
            apiClient = new APIClient(useAuthentication: true);
            response = await apiClient.Reset();
        }

        [When("the user performs a reset with invalid credentials")]
        public async Task WhenTheUserPerformsResetWithInvalidCredentials()
        {
            ApiCredentials.OverrideUsername = "special_user";
            ApiCredentials.OverridePassword = "special_password";
            apiClient = new APIClient(useAuthentication: true);
            response = await apiClient.Reset();
        }

        [When("the user performs a reset without authentication")]
        public async Task WhenTheUserPerformsResetWithoutAuthentication()
        {
            apiClient = new APIClient(useAuthentication: false);
            response = await apiClient.Reset();
        }

        [Then(@"the reset response status code should be (.*)")]
        public void ThenTheResetResponseStatusCodeShouldBe(int expectedStatusCode)
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(expectedStatusCode), $"Expected {expectedStatusCode} but got {(int)response.StatusCode}");
            CommonHelper.LogAndValidateResponse<dynamic>(response);
        }

        [AfterScenario]
        public void Cleanup()
        {
            apiClient?.Dispose();
        }
    }
}
