using EnseK_API_Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnseK_API_Automation
{
    public interface IApiClientFactory
    {
        APIClient Create(bool useAuthentication);

    }
}
