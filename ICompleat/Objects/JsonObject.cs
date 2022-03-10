using System.Net.Http.Json;
using System.Text.Json;

namespace ICompleat.Objects
{
    public abstract class JsonObject
    {
        #region Fields

        private static Random rnd = new Random();
        public JsonElement json;

        #endregion Fields

        #region Methods

        public static async Task<JsonElement> Execucte(string path = "", string method = "GET", object data = null, bool raisedAsRetry = false)
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod(method), Config._instance.url + path))
                {
                    request.Headers.Add("x-api-compleat-key", Config._instance.key);
                    request.Headers.Add("x-api-version", "1");

                    if (data != null)
                    {
                        request.Content = JsonContent.Create(data);
                    }

                    var response = await httpClient.SendAsync(request);

                    string body = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var j = JsonSerializer.Deserialize<JsonElement>(body);
                        //if (raisedAsRetry) Console.WriteLine("OK");
                        return j;
                    }
                    else
                    {
                        if (body.Contains("429"))
                        {
                            //if (!raisedAsRetry) Console.Write("Rate Limited, Trying Again");
                            //Console.Write(".");
                            Thread.Sleep(((int)rnd.NextDouble()*5000) + 1000);
                            return await Execucte(path, method, data, true);
                        }
                        throw new Exception(body);
                    }
                }
            }
        }

        #endregion Methods
    }
}