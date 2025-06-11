using EnseK_API_Automation.config;

namespace EnseK_API_Automation.constants
{
    public class ApiEnvironment
    {
        private static readonly Dictionary<string, string> BaseUrls = new()
    {
        { "DEV", "https://dev.ensek.io" },
        { "QA", "https://qacandidatetest.ensek.io" },
        { "PROD", "https://api.ensek.io" }
    };

        public static string BaseUrl =>
            BaseUrls.TryGetValue(EnvironmentConfig.CurrentEnvironment.ToUpper(), out var url)
            ? url
            : throw new Exception($"Base URL not defined for environment: {EnvironmentConfig.CurrentEnvironment}");
    }
}
