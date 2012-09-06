using System;
using System.Net;
using System.Net.Http;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Service.Factories;
using TeamNotification_Library.Service.Providers;

namespace TeamNotification_Library.Service.Http
{
    public class HttpClientHandlerGetter : IGetHttpClientHandler
    {
        private IProvideUser userProvider;
        private ICreateInstances<HttpClientHandler> httpClientHandlerFactory;
        private IProvideConfiguration<ServerConfiguration> serverConfigurationProvider;

        public HttpClientHandlerGetter(IProvideUser userProvider, ICreateInstances<HttpClientHandler> httpClientHandlerFactory, IProvideConfiguration<ServerConfiguration> serverConfigurationProvider)
        {
            this.userProvider = userProvider;
            this.serverConfigurationProvider = serverConfigurationProvider;

            this.httpClientHandlerFactory = httpClientHandlerFactory;
        }

        public HttpClientHandler GetHandler()
        {
            var handler = httpClientHandlerFactory.GetInstance();

            var user = userProvider.GetUser();
            if (user != null){
                handler.Credentials = new NetworkCredential(user.email, user.password);
                var userPass =  System.Text.Encoding.UTF8.GetBytes((user.email+":"+user.password));
                var encrypted = Convert.ToBase64String(userPass);
                handler.CookieContainer.Add(new Uri(serverConfigurationProvider.Get().Uri), new Cookie("authtoken", "Basic "+encrypted,"/"));
            }

            return handler;
        }
    }
}