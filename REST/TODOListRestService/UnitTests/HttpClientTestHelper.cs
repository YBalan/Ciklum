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

namespace Tests
{
    public static class HttpClientTestHelper
    {
        //public const string REST_SERVICE_START_URL = "http://localhost:8000/ToDoListRestAPIService.svc/";
        public const string REST_SERVICE_START_URL = "http://wcftodolistservice20180706114723.azurewebsites.net/ToDoListRestAPIService.svc/";
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

        public static string SendPost(string uri, string jsonData, out HttpStatusCode statusCode, out string description)
        {
            var strResult = string.Empty;
            var data = Encoding.ASCII.GetBytes(jsonData);

            var webrequest = (HttpWebRequest)WebRequest.Create(uri);

            webrequest.Method = "POST";
            webrequest.ContentType = "application/x-www-form-urlencoded";
            webrequest.ContentLength = data.Length;

            using (var newStream = webrequest.GetRequestStream())
            {
                newStream.Write(data, 0, data.Length);
            }

            description = string.Empty;
            statusCode = HttpStatusCode.BadRequest;

            using (var webresponse = (HttpWebResponse)webrequest.GetResponse())
            {
                statusCode = webresponse.StatusCode;
                description = webresponse.StatusDescription;

                using (var loResponseStream = new StreamReader(webresponse.GetResponseStream(), Encoding.UTF8))
                {
                    strResult = loResponseStream.ReadToEnd();
                }
            }

            return strResult;
        }
    }
}
