using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Service.Logging;

namespace TeamNotification_Library.Service.Http
{
    public class HttpRequestsClient : ISendHttpRequests
    {
        private ILog logger;

        private HttpClient httpClient
        {
            get
            {
                // TODO: Remove ServerCertificateValidationCallback after using the correct certificate. 
                ServicePointManager.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;
                return new HttpClient(httpClientHandlerGetter.GetHandler());
            }
        }

        private IGetHttpClientHandler httpClientHandlerGetter;
        readonly ISerializeJSON serializer;

        public HttpRequestsClient(ISerializeJSON serializer, IGetHttpClientHandler httpClientHandlerGetter, ILog logger)
        {
            this.serializer = serializer;
            this.httpClientHandlerGetter = httpClientHandlerGetter;
            this.logger = logger;
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
            return httpClient
                .GetStringAsync(uri)
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

        public void Post(IEnumerable<Tuple<string, HttpContent>> values)
        {
            logger.TryOrLog(() =>
                                {
                                    var backgroundWorker = new BackgroundWorker();
                                    backgroundWorker.WorkerSupportsCancellation = true;
                                    backgroundWorker.DoWork += (o, args) =>
                                    {
                                        values.Each(x => PostSync(x.Item1, x.Item2));
                                        args.Cancel = true;
                                    };
                                    backgroundWorker.RunWorkerAsync();                        
                                });
        }

        public HttpResponseMessage PostSync(string uri, HttpContent content)
        {
            return httpClient.PostAsync(uri, content).Result;
        }
    }
}