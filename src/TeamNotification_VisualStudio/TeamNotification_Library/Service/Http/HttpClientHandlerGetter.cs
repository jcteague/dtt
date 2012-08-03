using System.Net;
using System.Net.Http;
using TeamNotification_Library.Service.Factories;
using TeamNotification_Library.Service.Providers;

namespace TeamNotification_Library.Service.Http
{
    public class HttpClientHandlerGetter : IGetHttpClientHandler
    {
        private IProvideUser userProvider;
        private ICreateInstances<HttpClientHandler> httpClientHandlerFactory;

        public HttpClientHandlerGetter(IProvideUser userProvider, ICreateInstances<HttpClientHandler> httpClientHandlerFactory)
        {
            this.userProvider = userProvider;
            this.httpClientHandlerFactory = httpClientHandlerFactory;
        }

        public HttpClientHandler GetHandler()
        {
            var handler = httpClientHandlerFactory.GetInstance();

            var user = userProvider.GetUser();
            if(user != null)
                handler.Credentials = new NetworkCredential(user.email, user.password);

            return handler;
        }
    }
}