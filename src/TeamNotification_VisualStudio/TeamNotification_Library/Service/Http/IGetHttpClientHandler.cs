using System.Net;
using System.Net.Http;
using TeamNotification_Library.Service.Providers;

namespace TeamNotification_Library.Service.Http
{
    public interface IGetHttpClientHandler
    {
        HttpClientHandler GetHandler();
    }

    public class HttpClientHandlerGetter : IGetHttpClientHandler
    {
        private IProvideUser userProvider;

        public HttpClientHandlerGetter(IProvideUser userProvider)
        {
            this.userProvider = userProvider;
        }

        public HttpClientHandler GetHandler()
        {
            var user = userProvider.GetUser();
            var handler = new HttpClientHandler();
            handler.Credentials = new NetworkCredential(user.email, user.password);

            return handler;
        }
    }
}