using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnseK_API_Automation.Models
{
    public class ExpectedOrder
    {
        public string OrderId { get; set; }
        public string EnergyType { get; set; }
        public int Quantity { get; set; }
    }
}
