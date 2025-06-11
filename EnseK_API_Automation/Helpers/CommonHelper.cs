using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;

namespace EnseK_API_Automation.Helpers
{
    public class CommonHelper
    {
        /// <summary>
        /// Logs and validates any API response. If successful, deserializes the response and writes its contents.
        /// If unsuccessful, logs the status code and error message.
        /// </summary>
        /// <typeparam name="T">Type to deserialize the response into (e.g., Order, Inventory, etc.)</typeparam>
        /// <param name="response">The RestSharp response to validate and log.</param>
        /// <param name="onSuccess">Optional callback to handle deserialized object on success.</param>
        public static void LogAndValidateResponse<T>(RestResponse response, Action<T>? onSuccess = null)
        {
            if (response.IsSuccessful)
            {
                try
                {
                    var result = JsonConvert.DeserializeObject<T>(response.Content!);
                    Assert.IsNotNull(result, $"❌ Response deserialization into {typeof(T).Name} failed.");
                    TestContext.WriteLine($"✅ Response deserialized: {JsonConvert.SerializeObject(result, Formatting.Indented)}");

                    onSuccess?.Invoke(result!);
                }
                catch (Exception ex)
                {
                    Assert.Fail($"❌ Exception while deserializing response into {typeof(T).Name}: {ex.Message}");
                }
            }
            else
            {
                TestContext.WriteLine($"❌ Request failed: {response.StatusCode} - {response.ErrorMessage ?? "No error message"}");
            }

        }

    }
}
