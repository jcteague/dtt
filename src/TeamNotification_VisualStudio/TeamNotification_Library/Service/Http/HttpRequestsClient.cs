using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace TeamNotification_Library.Service.Http
{
    public class HttpRequestsClient : ISendHttpRequests
    {
        readonly ISerializeJSON serializer;
        readonly HttpClient httpClient;

        public HttpRequestsClient(ISerializeJSON serializer)
        {
            this.serializer = serializer;
            httpClient = new HttpClient();
        }

        public void Get(string uri)
        {
            httpClient.GetAsync(uri);
        }

        public void Post(string uri, params KeyValuePair<string, string>[] values)
        {
            httpClient.PostAsync(uri, new FormUrlEncodedContent(values));
        }

        public Task<T> Get<T>(string uri) where T : class
        {
            return httpClient.GetStringAsync(uri)
                .ContinueWith(response => serializer.Deserialize<T>(response.Result));
        }
        
        public void Get(string uri, Action<Task<string>> action)
        {
            httpClient.GetStringAsync(uri).ContinueWith(action);
        }

//        public Task<HttpResponseMessage> Post<T>(string uri, HttpContent content) where T : class
//        {
//            return httpClient.PostAsync(uri, content);
//        }

        public Task<T> Post<T>(string uri, HttpContent content) where T : class
        {
            return httpClient.PostAsync(uri, content)
                    .ContinueWith(response => serializer.Deserialize<T>(response.Result.Content.ReadAsStringAsync().Result));
        }
    }
}