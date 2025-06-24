using EnseK_API_Automation.Auth;
using EnseK_API_Automation.constants;
using RestSharp;

namespace EnseK_API_Automation
{
    public class APIClient : IEnsekClientAPI, IDisposable
    {

        readonly RestClient restClient;
        readonly string baseUrl = ApiEnvironment.BaseUrl;

        public APIClient(bool useAuthentication = true)
        {
            var options = new RestClientOptions(ApiEnvironment.BaseUrl);

            if (useAuthentication)
            {
                options.Authenticator = new ApiAuthenticator();
            }

            restClient = new RestClient(options);
        }

        public async Task<RestResponse> BuyProduct(String productId, String quantity)
        {
            var request = new RestRequest(Endpoints.BUY_PRODUCT, Method.Put);
            request.AddUrlSegment("productId", productId);
            request.AddUrlSegment("quantity", quantity);
            return await ExecuteAsync(request);

        }

        public async Task<RestResponse> DeleteOrder(string orderid)
        {
            var request = new RestRequest(Endpoints.DELETE_ORDER, Method.Delete);
            request.AddUrlSegment("orderId", orderid);
            return await ExecuteAsync(request);
        }

        public void Dispose()
        {
            restClient?.Dispose();
        }

        public async Task<RestResponse> GetEnergyData()
        {
            var request = new RestRequest(Endpoints.GET_ENERGY, Method.Get);
            return await ExecuteAsync(request);
        }

        public async Task<RestResponse> GetOrder(String orderId)
        {
            var request = new RestRequest(Endpoints.GET_ORDER, Method.Get);
            request.AddUrlSegment(orderId, orderId);
            return await ExecuteAsync(request);
        }

        public async Task<RestResponse> GetOrders()
        {
            var request = new RestRequest(Endpoints.GET_ORDERS, Method.Get);
            return await ExecuteAsync(request);
        }

        public async Task<RestResponse> Login<T>(T payload) where T : class
        {
            var request = new RestRequest(Endpoints.LOGIN, Method.Post);
            request.AddBody(payload);
            return await ExecuteAsync(request);
        }

        public async Task<RestResponse> Reset()
        {
            var request = new RestRequest(Endpoints.RESET_ORDER, Method.Post);
            return await ExecuteAsync(request);
        }

        public async Task<RestResponse> UpdateOrder<T>(T payload, string orderId) where T : class
        {
            var request = new RestRequest(Endpoints.UPDATE_ORDER, Method.Put);
            request.AddBody(payload);
            request.AddUrlSegment("orderId", orderId);
            return await ExecuteAsync(request);
        }

        private async Task<RestResponse> ExecuteAsync(RestRequest request)
        {
            try
            {
                var response = await restClient.ExecuteAsync(request);

                if (!response.IsSuccessful)
                {
                    throw new HttpRequestException($"API call failed: {(int)response.StatusCode} {response.StatusDescription}");
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("API request failed.", ex);
            }
        }
    }
}
