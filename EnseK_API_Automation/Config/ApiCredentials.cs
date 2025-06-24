/// <summary>
/// Provides API credentials (username and password) based on the current environment.
/// Supports default environment-based credentials and optional per-test overrides.
/// </summary>
/// <remarks>
/// Usage:
/// - Access credentials via ApiCredentials.Username and ApiCredentials.Password.
/// - Use OverrideUsername and OverridePassword to temporarily override credentials during tests.
/// - Call ClearOverrides() to reset overrides and use environment defaults.
/// </remarks>
/// <example>
/// string user = ApiCredentials.Username;
/// string pass = ApiCredentials.Password;
///
/// ApiCredentials.OverrideUsername = "custom_user";
/// ApiCredentials.OverridePassword = "custom_pass";
///
/// ApiCredentials.ClearOverrides(); // back to default
/// </example>

namespace EnseK_API_Automation.config
{
    public static class ApiCredentials
    {
        /// <summary>
        /// Default username/password combinations per environment.
        /// </summary>
        private static readonly Dictionary<string, (string Username, string Password)> Credentials = new()
        {
            { "DEV", ("dev_user", "dev_pass") },
            { "QA", ("test", "testing") },
            { "PROD", ("prod_user", "secure_prod_pass") }
        };

        /// <summary>
        /// Optional override for username (used during tests).
        /// </summary>
        public static string? OverrideUsername { get; set; }

        /// <summary>
        /// Optional override for password (used during tests).
        /// </summary>
        public static string? OverridePassword { get; set; }

        /// <summary>
        /// Gets the active username. Uses override if set, otherwise the default for the current environment.
        /// </summary>
        public static string Username =>
            OverrideUsername ?? GetDefault().Username;

        /// <summary>
        /// Gets the active password. Uses override if set, otherwise the default for the current environment.
        /// </summary>
        public static string Password =>
            OverridePassword ?? GetDefault().Password;

        /// <summary>
        /// Retrieves default credentials for the current environment.
        /// Throws an exception if no credentials are found.
        /// </summary>
        private static (string Username, string Password) GetDefault()
        {
            if (Credentials.TryGetValue(EnvironmentConfig.CurrentEnvironment.ToUpper(), out var creds))
                return creds;

            throw new Exception($"No credentials defined for environment: {EnvironmentConfig.CurrentEnvironment}");
        }

        /// <summary>
        /// Clears the username and password overrides, reverting to environment defaults.
        /// </summary>
        public static void ClearOverrides()
        {
            OverrideUsername = null;
            OverridePassword = null;
        }
    }
}
