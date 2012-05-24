namespace TeamNotification_Library.Service.Http
{
    public class HttpClient : ICommunicateUsingHttp
    {
        readonly System.Net.Http.HttpClient httpClient;

        public HttpClient()
        {
            httpClient = new System.Net.Http.HttpClient();
        }

        public void Get(string uri)
        {
            httpClient.GetAsync(uri);
        }
    }
}