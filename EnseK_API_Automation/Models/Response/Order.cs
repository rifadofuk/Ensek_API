using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnseK_API_Automation.Models.Response
{
    public class Order
    {
        public string Id { get; set; }
        public string Fuel { get; set; }
        public int Quantity { get; set; }
        public string Time { get; set; } // Y
    }
}
