using EnseK_API_Automation.Auth;
using EnseK_API_Automation.constants;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return await restClient.ExecuteAsync(request);

        }

        public async Task<RestResponse> DeleteOrder(string orderid)
        {
            var request = new RestRequest(Endpoints.DELETE_ORDER, Method.Delete);
            request.AddUrlSegment("orderId", orderid);
            return await restClient.ExecuteAsync(request);
        }

        public void Dispose()
        {
            restClient?.Dispose();
        }

        public async Task<RestResponse> GetEnergyData()
        {
            var request = new RestRequest(Endpoints.GET_ENERGY, Method.Get);
            return await restClient.ExecuteAsync(request);
        }

        public async Task<RestResponse> GetOrder(String orderId)
        {
            var request = new RestRequest(Endpoints.GET_ORDER, Method.Get);
            request.AddUrlSegment(orderId, orderId);
            return await restClient.ExecuteAsync(request);
        }

        public async Task<RestResponse> GetOrders()
        {
            var request = new RestRequest(Endpoints.GET_ORDERS, Method.Get);
            return await restClient.ExecuteAsync(request);
        }

        public async Task<RestResponse> Login<T>(T payload) where T : class
        {
            var request = new RestRequest(Endpoints.LOGIN, Method.Post);
            request.AddBody(payload);
            return await restClient.ExecuteAsync(request);
        }

        public async Task<RestResponse> Reset()
        {
            var request = new RestRequest(Endpoints.RESET_ORDER, Method.Post);
            return await restClient.ExecuteAsync(request);
        }

        public async Task<RestResponse> UpdateOrder<T>(T payload, string orderId) where T : class
        {
            var request = new RestRequest(Endpoints.UPDATE_ORDER,Method.Put);
            request.AddBody(payload);
            request.AddUrlSegment("orderId", orderId);
            return await restClient.ExecuteAsync<T>(request);
        }
    }
}
