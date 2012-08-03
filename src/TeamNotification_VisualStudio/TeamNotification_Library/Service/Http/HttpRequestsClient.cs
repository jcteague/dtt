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
        private HttpClient httpClient;
        private IGetHttpClientHandler httpClientHandlerGetter;
        readonly ISerializeJSON serializer;

        public HttpRequestsClient(ISerializeJSON serializer, IProvideUser userProvider, IGetHttpClientHandler httpClientHandlerGetter)
        {
            this.serializer = serializer;
            this.httpClientHandlerGetter = httpClientHandlerGetter;
            httpClient = new HttpClient(httpClientHandlerGetter.GetHandler());
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

        public Task<T> Post<T>(string uri, HttpContent content) where T : class
        {
            return httpClient.PostAsync(uri, content)
                    .ContinueWith(response => serializer.Deserialize<T>(response.Result.Content.ReadAsStringAsync().Result));
        }
    }
}