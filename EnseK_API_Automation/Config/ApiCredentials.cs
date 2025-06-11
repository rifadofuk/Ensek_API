using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnseK_API_Automation.config
{
    public static class ApiCredentials
    {
        // Default per-environment credentials
        private static readonly Dictionary<string, (string Username, string Password)> Credentials = new()
    {
        { "DEV", ("dev_user", "dev_pass") },
        { "QA", ("test", "testing") },
        { "PROD", ("prod_user", "secure_prod_pass") }
    };

        // Optional overrides (per test)
        public static string? OverrideUsername { get; set; }
        public static string? OverridePassword { get; set; }

        public static string Username =>
            OverrideUsername ?? GetDefault().Username;

        public static string Password =>
            OverridePassword ?? GetDefault().Password;

        private static (string Username, string Password) GetDefault()
        {
            if (Credentials.TryGetValue(EnvironmentConfig.CurrentEnvironment.ToUpper(), out var creds))
                return creds;

            throw new Exception($"No credentials defined for environment: {EnvironmentConfig.CurrentEnvironment}");
        }

        public static void ClearOverrides()
        {
            OverrideUsername = null;
            OverridePassword = null;
        }
    }

}
