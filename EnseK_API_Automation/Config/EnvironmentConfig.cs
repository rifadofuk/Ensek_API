namespace EnseK_API_Automation.config
{
    public class EnvironmentConfig
    {
        // Set this at the start of your test run or load from env var
        public static string CurrentEnvironment { get; set; } = "QA";
    }
}
