using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ICompleat.Objects
{
    public abstract class JsonObject
    {
        #region Fields

        protected JsonElement json;

        #endregion Fields

        #region Methods

        public static async Task<JsonElement> Execucte(string path = "", string method = "GET")
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod(method), Config._instance.url + path))
                {
                    request.Headers.Add("x-api-compleat-key", Config._instance.key);
                    request.Headers.Add("x-api-version", "1");
                    var response = await httpClient.SendAsync(request);

                    string body = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return JsonSerializer.Deserialize<JsonElement>(body);
                    }
                    else
                    {
                        response.EnsureSuccessStatusCode();
                        throw new Exception(body);
                    }
                }
            }
        }

        #endregion Methods
    }
}