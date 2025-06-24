using Microsoft.Extensions.DependencyInjection;
using ENSEK_API_Automation;
using RestSharp;
using EnseK_API_Automation;

namespace ENSEK_API_Automation.Tests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IApiClientFactory, ApiClientFactory>();

        }
    }
}
