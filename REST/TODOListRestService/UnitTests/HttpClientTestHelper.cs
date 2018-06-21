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

        public static string SendPost2(string uri, string jsonData)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            string strResult = string.Empty;
            byte[] data = encoding.GetBytes(jsonData);
            // declare httpwebrequet wrt url defined above
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(uri);
            // set method as post
            webrequest.Method = "POST";
            // set content type
            webrequest.ContentType = "application/x-www-form-urlencoded";
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
                using (StreamReader loResponseStream = new StreamReader(webresponse.GetResponseStream(), Encoding.UTF8))
                {
                    // read string from stream data
                    strResult = loResponseStream.ReadToEnd();
                    // close the stream object
                }
                // close the response object
            }
            // below steps remove unwanted data from response string
            //strResult = strResult.Replace("</string>", "");


            return strResult;
        }

        public static string SendPost(string uri, string jsonData)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(uri),
                };

                var httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var httpResponseMessage = httpClient.PostAsync(uri, httpContent);

                var result = string.Empty;

                httpResponseMessage.Wait();
                if (httpResponseMessage.Result.StatusCode == HttpStatusCode.OK)
                {
                    var task = httpResponseMessage.Result.Content.ReadAsStringAsync();
                    task.Wait();
                    result = task.Result;
                }

                return result;
            }
        }

        private static async Task<string> SendPostAsync(string uri, string jsonData)
        {
            string responseText = string.Empty;
            using (var sendHttpClient = new HttpClient())
            {
                sendHttpClient.BaseAddress = new Uri(uri);
                sendHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var pHeaders = new Dictionary<string, string>();

                var content = jsonData;


                using (var response = await Request(sendHttpClient, HttpMethod.Post, uri, content, pHeaders))
                {
                    using (var httpContent = response.Content)
                    {
                        responseText = await httpContent.ReadAsStringAsync();
                    }
                }
            }

            return responseText;
        }

        private static Task<HttpResponseMessage> Request(HttpClient client, HttpMethod pMethod, string pUrl, string pJsonContent, Dictionary<string, string> pHeaders)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = pMethod,
                RequestUri = new Uri(pUrl)
            };

            foreach (var head in pHeaders)
            {
                httpRequestMessage.Headers.Add(head.Key, head.Value);
            }
            HttpContent httpContent = null;
            Task<HttpResponseMessage> message = null;

            switch (pMethod.Method)
            {
                case "POST":
                    httpContent = new StringContent(pJsonContent, Encoding.UTF8, "application/json");
                    httpRequestMessage.Content = httpContent;
                    message = client.SendAsync(httpRequestMessage);
                    break;
            }

            message.Wait();
            if (message.Result.StatusCode == HttpStatusCode.OK)
            {
                var task = message.Result.Content.ReadAsStringAsync();
                task.Wait();
                string strRes = task.Result;
            }
            return message;
        }
    }
}
