using System.Net.Http;

namespace TeamNotification_Library.Service.Http
{
    public class HttpRequestsClient : ISendHttpRequests
    {
        readonly HttpClient httpClient;

        public HttpRequestsClient()
        {
            httpClient = new HttpClient();
        }

        public void Get(string uri)
        {
            httpClient.GetAsync(uri);
        }
    }
}