using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnseK_API_Automation.config
{
    public class EnvironmentConfig
    {
        // Set this at the start of your test run or load from env var
        public static string CurrentEnvironment { get; set; } = "QA";
    }
}
