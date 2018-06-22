using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public static class HttpClientTestHelper
    {
        public static string SendGet(string uri)
        {
            using (var httpClient = new HttpClient())
            {
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(uri),
                };
                var httpResponseMessage = httpClient.GetAsync(uri);

                httpResponseMessage.Wait();

                var task = httpResponseMessage.Result.Content.ReadAsStringAsync();
                task.Wait();
                return task.Result;
            }
        }

        public static string SendPost(string uri, string jsonData)
        {
            var strResult = string.Empty;
            var data = Encoding.ASCII.GetBytes(jsonData);
            // declare httpwebrequet wrt url defined above
            var webrequest = (HttpWebRequest)WebRequest.Create(uri);
            {
                // set method as post
                webrequest.Method = "POST";
                // set content type
                webrequest.ContentType = "application/json";
                // set content length
                webrequest.ContentLength = data.Length;
                // get stream data out of webrequest object
                using (var newStream = webrequest.GetRequestStream())
                {
                    newStream.Write(data, 0, data.Length);
                }
                // declare & read response from service
                using (var webresponse = (HttpWebResponse)webrequest.GetResponse())
                {
                    // read response stream from response object
                    using (var loResponseStream = new StreamReader(webresponse.GetResponseStream(), Encoding.UTF8))
                    {
                        // read string from stream data
                        strResult = loResponseStream.ReadToEnd();
                        // close the stream object
                    }
                    // close the response object
                }
            }

            return strResult;
        }
    }
}
