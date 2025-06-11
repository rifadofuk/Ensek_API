using EnseK_API_Automation.Models;
using EnseK_API_Automation.Models.Response;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace EnseK_API_Automation.Helpers
{
    /// <summary>
    /// Provides helper methods for purchasing fuels, validating orders, and analyzing order data.
    /// </summary>
    public static class OrderHelper
    {
        /// <summary>
        /// Attempts to purchase 1 unit of each available fuel type from the provided inventory.
        /// Records any errors encountered and returns a list of successfully placed orders.
        /// </summary>
        /// <param name="client">API client with authentication</param>
        /// <param name="inventory">The inventory object containing available energy sources</param>
        /// <param name="quantityToBuy">The quantity to attempt purchasing per fuel</param>
        /// <param name="errors">List to accumulate any errors during the purchase process</param>
        /// <returns>List of successfully placed expected orders</returns>
        public static async Task<List<ExpectedOrder>> BuyAvailableFuels(APIClient client, EnergyInventory inventory, int quantityToBuy, List<string> errors)
        {
            var fuels = new Dictionary<string, EnergySource>
            {
                ["Electric"] = inventory.Electric,
                ["Gas"] = inventory.Gas,
                ["Nuclear"] = inventory.Nuclear,
                ["Oil"] = inventory.Oil
            };

            var placedOrders = new List<ExpectedOrder>();

            foreach (var (fuelName, source) in fuels)
            {
                var order = await TryPurchaseFuelAndCaptureOrder(fuelName, source, quantityToBuy, errors);
                if (order != null)
                    placedOrders.Add(order);
            }

            return placedOrders;
        }

        /// <summary>
        /// Attempts to purchase the given quantity of a specific fuel type and extract the order ID from the API response.
        /// </summary>
        /// <param name="fuelName">Name of the fuel being purchased (for display/logging)</param>
        /// <param name="source">The specific energy source from the inventory</param>
        /// <param name="quantityToBuy">Quantity to purchase</param>
        /// <param name="errors">List to collect any error messages encountered</param>
        /// <returns>An ExpectedOrder object if successful, otherwise null</returns>
        private static async Task<ExpectedOrder?> TryPurchaseFuelAndCaptureOrder(string fuelName, EnergySource source, int quantityToBuy, List<string> errors)
        {
            try
            {
                var client = new APIClient();
                var response = await client.BuyProduct(source.Energy_Id.ToString(), quantityToBuy.ToString());
                var statusCode = (int)response.StatusCode;
                var message = JsonConvert.DeserializeObject<dynamic>(response.Content)?.message?.ToString();

                TestContext.WriteLine($"🧪 {fuelName}: {message}");

                if (source.Quantity_Of_Units < quantityToBuy || statusCode != 200 || string.IsNullOrEmpty(message))
                    return null;

                // Extract the order ID from the returned message using a regex
                var match = Regex.Match(message, @"order\s?id\s?is\s?([a-f0-9\-]{36})", RegexOptions.IgnoreCase);
                if (!match.Success)
                {
                    errors.Add($"❌ {fuelName}: Order ID not found in message.");
                    return null;
                }

                var orderId = match.Groups[1].Value;
                return new ExpectedOrder
                {
                    OrderId = orderId,
                    EnergyType = fuelName,
                    Quantity = quantityToBuy
                };
            }
            catch (Exception ex)
            {
                errors.Add($"💥 {fuelName}: Exception during purchase — {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Compares the placed orders with actual orders fetched from the system and logs any mismatches.
        /// </summary>
        /// <param name="placedOrders">List of expected orders placed during the test</param>
        /// <param name="actualOrders">List of orders returned by the system</param>
        /// <param name="errors">List to record any validation errors</param>
        public static void ValidatePlacedOrders(List<ExpectedOrder> placedOrders, List<Order> actualOrders, List<string> errors)
        {
            foreach (var expected in placedOrders)
            {
                var match = actualOrders.FirstOrDefault(o =>
                    o.Id.Equals(expected.OrderId, StringComparison.OrdinalIgnoreCase) &&
                    o.Fuel.Equals(expected.EnergyType, StringComparison.OrdinalIgnoreCase) &&
                    o.Quantity == expected.Quantity);

                if (match == null)
                {
                    errors.Add($"❌ Order missing: ID={expected.OrderId}, Fuel={expected.EnergyType}, Qty={expected.Quantity}");
                }
            }
        }

        /// <summary>
        /// Counts the number of orders whose creation date is before the current UTC date.
        /// </summary>
        /// <param name="orders">List of orders to evaluate</param>
        /// <returns>Number of orders created before today</returns>
        public static int CountOrdersBeforeToday(List<Order> orders)
        {
            var today = DateTime.UtcNow.Date;

            return orders.Count(o =>
            {
                if (DateTime.TryParse(o.Time, out var orderTime))
                {
                    return orderTime.Date < today;
                }
                return false;
            });
        }


        /// <summary>
        /// Validates the response message after purchasing a specific fuel. It checks cost, unit type, and order structure.
        /// </summary>
        /// <param name="fuelName">Fuel name</param>
        /// <param name="source">Energy source object from inventory</param>
        /// <param name="quantityToBuy">Quantity to buy</param>
        /// <param name="errors">List to append any validation errors</param>
        public static async Task ValidateFuelPurchaseDetails(string fuelName, EnergySource source, int quantityToBuy, List<string> errors)
        {
            try
            {
                var client = new APIClient(useAuthentication: true);
                var response = await client.BuyProduct(source.Energy_Id.ToString(), quantityToBuy.ToString());
                var statusCode = (int)response.StatusCode;

                var expectedCost = Math.Round(source.Price_Per_Unit * quantityToBuy, 2);
                var expectedRemaining = source.Quantity_Of_Units - quantityToBuy;

                var message = JsonConvert.DeserializeObject<dynamic>(response.Content)?.message?.ToString();
                TestContext.WriteLine($"🧪 {fuelName}: Response => {message}");

                if (statusCode != 200)
                {
                    errors.Add($"❌ {fuelName}: Expected 200 OK but got {statusCode}");
                    return;
                }

                if (source.Quantity_Of_Units >= quantityToBuy)
                {
                    if (string.IsNullOrWhiteSpace(message))
                    {
                        errors.Add($"❌ {fuelName}: Response message is empty.");
                        return;
                    }

                    if (!message.Contains($"{quantityToBuy} {source.Unit_Type}"))
                        errors.Add($"❌ {fuelName}: Quantity/unit mismatch in message.");

                    if (!message.Contains($"cost of {expectedCost}"))
                        errors.Add($"❌ {fuelName}: Expected cost {expectedCost} not found in message.");

                    if (!message.Contains($"{expectedRemaining} units remaining"))
                        errors.Add($"❌ {fuelName}: Remaining quantity {expectedRemaining} not found in message.");

                    if (!Regex.IsMatch(message, @"order\s?id\s?is\s?[a-f0-9\-]{36}", RegexOptions.IgnoreCase))
                        errors.Add($"❌ {fuelName}: Order ID missing or malformed.");
                }
                else
                {
                    if (!message.Contains($"There is no {fuelName} fuel to purchase!"))
                        errors.Add($"❌ {fuelName}: Out-of-stock message incorrect.");
                }
            }
            catch (Exception ex)
            {
                errors.Add($"💥 {fuelName}: Exception occurred — {ex.Message}");
            }
        }

    }
}
