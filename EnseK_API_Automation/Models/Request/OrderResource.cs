using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnseK_API_Automation.Models.Request
{
    public class OrderResource
    {
        public string id { get; set; }  // Default value can be set in the constructor or elsewhere
        public int quantity { get; set; } = 0;  // Default value of 0
        public int energy_id { get; set; }
    }
}
