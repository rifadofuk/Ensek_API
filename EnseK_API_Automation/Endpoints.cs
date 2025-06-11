namespace EnseK_API_Automation
{
    internal class Endpoints
    {
        public static readonly string BUY_PRODUCT = "/ENSEK/buy/{productId}/{quantity}";                    // PUT /ENSEK/buy/{productId}/{quantity}
        public static readonly string GET_ENERGY = "/ENSEK/energy";                 // GET /ENSEK/energy
        public static readonly string LOGIN = "/ENSEK/login";                       // POST /ENSEK/login
        public static readonly string GET_ORDERS = "/ENSEK/orders";
        public static readonly string UPDATE_ORDER = "/ENSEK/orders/{orderId}";
        public static readonly string DELETE_ORDER = "/ENSEK/orders/{orderId}";
        public static readonly string GET_ORDER = "/ENSEK/orders/{orderId}";
        public static readonly string RESET_ORDER = "/ENSEK/reset";



    }
}
