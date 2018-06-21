using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public static class HttpClientTestHelper
    {
        public static string Send(string uri, HttpMethod method)
        {
            using (var httpClient = new HttpClient())
            {
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = method,
                    RequestUri = new Uri(uri),
                };                                               

                //var httpResponseMessage = httpClient.SendAsync(httpRequestMessage);
                var httpResponseMessage = httpClient.GetAsync(uri);

                httpResponseMessage.Wait();
                //if (httpResponseMessage.Result.StatusCode == HttpStatusCode.OK)
                {
                    var task = httpResponseMessage.Result.Content.ReadAsStringAsync();
                    task.Wait();
                    return task.Result;
                }
            }
        }

        public static TResult ParseJson<TResult>(string jsonRes) where TResult : class, new()
        {
            var jsonSerSettings = new DataContractJsonSerializerSettings();
            var jsonSerializer = new DataContractJsonSerializer(typeof(TResult), jsonSerSettings);

            TResult root = null;
            try
            {
                using (var memStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonRes)))
                {
                    root = jsonSerializer.ReadObject(memStream) as TResult;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }

            return root;
        }
    }
}
