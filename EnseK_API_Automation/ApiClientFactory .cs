using EnseK_API_Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnseK_API_Automation
{
    public class ApiClientFactory : IApiClientFactory
    {
        public APIClient Create(bool useAuthentication)
        {
            return new APIClient(useAuthentication);
        }
    }
}
