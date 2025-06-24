using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace Ensek_API_Client.Helpers
{
    public class JsonHelper
    {
        public static T DeserilizeJson<T>(string jsonContent)
        {
            if (string.IsNullOrEmpty(jsonContent))
            {
                throw new ArgumentNullException("Response Is Null Or Empty");
            }
            try
            {
                return JsonConvert.DeserializeObject<T>(jsonContent);

            }
            catch (JsonException js )
            {
                throw new ApplicationException($"❌ Failed to deserialize JSON to {typeof(T).Name}.", js);


            }

        }
    }
}
