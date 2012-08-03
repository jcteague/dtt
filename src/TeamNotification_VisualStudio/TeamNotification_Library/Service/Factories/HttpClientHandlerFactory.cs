using System.Net.Http;

namespace TeamNotification_Library.Service.Factories
{
    public class HttpClientHandlerFactory : ICreateInstances<HttpClientHandler>
    {
        public HttpClientHandler GetInstance()
        {
            return new HttpClientHandler();
        }
    }
}