using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnseK_API_Automation
{
    internal interface IEnsekClientAPI
    {
        Task<RestResponse> BuyProduct(String productId, String quantity);
        Task<RestResponse> GetEnergyData();
        Task<RestResponse> Login<T>(T payload) where T : class;
        Task<RestResponse> GetOrders();
        Task<RestResponse> DeleteOrder(string orderid);
        Task<RestResponse> UpdateOrder<T>(T payload, string orderId) where T : class;
        Task<RestResponse> GetOrder(String orderId);
        Task<RestResponse> Reset();

    }
}
