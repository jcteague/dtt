using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TeamNotification_Library.Service.Providers;

namespace TeamNotification_Library.Service.Http
{
    public class HttpRequestsClient : ISendHttpRequests
    {
        readonly ISerializeJSON serializer;
        private HttpClient httpClient;
        readonly IProvideUser userProvider;

        public HttpRequestsClient(ISerializeJSON serializer, IProvideUser userProvider)
        {
            this.serializer = serializer;
            //httpClient = new HttpClient();
            this.userProvider = userProvider;
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
            var user = userProvider.GetUser();
            using (var handler = new HttpClientHandler())
            {
                handler.Credentials = new NetworkCredential(user.email,user.password);
                this.httpClient = new HttpClient(handler);
                return httpClient.GetStringAsync(uri)
                    .ContinueWith(response => serializer.Deserialize<T>(response.Result));
            }
        }
        
        public void Get(string uri, Action<Task<string>> action)
        {
            httpClient.GetStringAsync(uri).ContinueWith(action);
        }

        public Task<T> Post<T>(string uri, HttpContent content) where T : class
        {
            return httpClient.PostAsync(uri, content)
                    .ContinueWith(response => serializer.Deserialize<T>(response.Result.Content.ReadAsStringAsync().Result));
        }
    }
}