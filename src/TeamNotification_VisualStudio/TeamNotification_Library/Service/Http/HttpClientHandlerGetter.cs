using System.Net;
using System.Net.Http;
using TeamNotification_Library.Service.Providers;

namespace TeamNotification_Library.Service.Http
{
    public class HttpClientHandlerGetter : IGetHttpClientHandler
    {
        private IProvideUser userProvider;

        public HttpClientHandlerGetter(IProvideUser userProvider)
        {
            this.userProvider = userProvider;
        }

        public HttpClientHandler GetHandler()
        {
            var handler = new HttpClientHandler();

            var user = userProvider.GetUser();
            if(user != null)
                handler.Credentials = new NetworkCredential(user.email, user.password);

            return handler;
        }
    }
}