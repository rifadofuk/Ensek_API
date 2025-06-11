using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENSEK_Test.steps
{
    using Reqnroll;
    using EnseK_API_Automation;
    using EnseK_API_Automation.config;
    using EnseK_API_Automation.Helpers;
    using NUnit.Framework;
    using RestSharp;

    namespace ENSEK_Test.steps
    {
        [Binding]
        public class CommonSetupSteps
        {
            private APIClient apiClient;

            [Given(@"the test environment is set to ""(.*)""")]
            public void GivenTheTestEnvironmentIsSetTo(string environment)
            {
                EnvironmentConfig.CurrentEnvironment = environment;
                ApiCredentials.ClearOverrides();
                apiClient = new APIClient(useAuthentication: true);
            }

            [Given("the system is reset to a clean state")]
            public async Task GivenTheSystemIsResetToACleanState()
            {
                var response = await apiClient.Reset();
                Assert.That((int)response.StatusCode, Is.EqualTo(200), "Reset failed.");
            }

            [AfterScenario]
            public void Cleanup()
            {
                apiClient?.Dispose();
            }
        }
    }

}
