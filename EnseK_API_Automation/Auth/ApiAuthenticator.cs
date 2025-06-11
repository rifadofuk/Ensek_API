using EnseK_API_Automation.config;
using EnseK_API_Automation.constants;
using EnseK_API_Automation.Models.Response;
using RestSharp;
using RestSharp.Authenticators;

namespace EnseK_API_Automation.Auth
{
    public class ApiAuthenticator : AuthenticatorBase
    {
        readonly string baseUrl = ApiEnvironment.BaseUrl;
        readonly string user = ApiCredentials.Username;
        readonly string password = ApiCredentials.Password;
        public ApiAuthenticator() : base("")
        {
        }

        protected override async ValueTask<Parameter> GetAuthenticationParameter(string accessToken)
        {
            var token = string.IsNullOrEmpty(Token) ? await GetToken() : Token;
            return new HeaderParameter(KnownHeaders.Authorization, token);


        }

        private async Task<string> GetToken()
        {
            var options = new RestClientOptions(baseUrl);
            var client = new RestClient(options);

            var request = new RestRequest(Endpoints.LOGIN, Method.Post);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");

            var credentials = new
            {
                username = user,
                password = password
            };

            request.AddJsonBody(credentials);

            // Use ExecutePostAsync to get status code
            var response = await client.ExecutePostAsync<TokenResponse>(request);

            // Fail on 401 Unauthorized
            //if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            //{
            //    Assert.Fail("❌ Authentication failed: 401 Unauthorized. Check username/password.");
            //}

            // Handle other failures or missing token
            //if (!response.IsSuccessful || response.Data == null || string.IsNullOrEmpty(response.Data.Access_token))
            //{
            //    Assert.Fail($"❌ Failed to retrieve token. Status: {response.StatusCode}, Body: {response.Content}");
            //}

            return $"Bearer {response.Data.Access_token}";
        }
    }
}

