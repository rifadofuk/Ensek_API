﻿namespace ENSEK_Test.steps
{
    using EnseK_API_Automation;
    using EnseK_API_Automation.config;
    using Reqnroll;

    [Binding]
    public sealed class GlobalHooks
    {
        private APIClient apiClient;

        [BeforeScenario("@api")]
        public async Task SetEnvironmentAndResetSystem()
        {
            string env = Environment.GetEnvironmentVariable("ENSEK_ENV") ?? "QA";
            EnvironmentConfig.CurrentEnvironment = env;
            ApiCredentials.ClearOverrides();

            apiClient = new APIClient(useAuthentication: true);
            var response = await apiClient.Reset();
            if ((int)response.StatusCode != 200)
            {
                throw new Exception("❌ Failed to reset system before scenario.");
            }
        }

        [AfterScenario("@api")]
        public void DisposeApiClient()
        {
            apiClient?.Dispose();
        }
    }

}
