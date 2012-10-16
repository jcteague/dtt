using System;
using System.Net;
using System.Net.Http;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Service.Factories;
using TeamNotification_Library.Service.FileSystem;
using TeamNotification_Library.Service.Providers;
using TeamNotification_Library.Extensions;

namespace TeamNotification_Library.Service.Http
{
    public class HttpClientHandlerGetter : IGetHttpClientHandler
    {
        private IProvideUser userProvider;
        private ICreateInstances<HttpClientHandler> httpClientHandlerFactory;
        private IProvideConfiguration<ServerConfiguration> serverConfigurationProvider;
        private IHandleEncoding encoder;
        public HttpClientHandlerGetter(IProvideUser userProvider, ICreateInstances<HttpClientHandler> httpClientHandlerFactory, IProvideConfiguration<ServerConfiguration> serverConfigurationProvider, IHandleEncoding encoder)
        {
            this.userProvider = userProvider;
            this.serverConfigurationProvider = serverConfigurationProvider;
            this.encoder = encoder; 
            this.httpClientHandlerFactory = httpClientHandlerFactory;
        }

        public HttpClientHandler GetHandler()
        {
            var handler = httpClientHandlerFactory.GetInstance();

            var user = userProvider.GetUser();
            if (user.IsNotNull())
            {
                handler.Credentials = new NetworkCredential(user.email, user.password);
                var encrypted = encoder.Encode(user.email + ":" + user.password);
                var uri = new Uri(serverConfigurationProvider.Get().Uri);
                var cookie = new Cookie("authtoken", "Basic " + encrypted, "/");
                handler.CookieContainer.Add(uri, cookie);
            }
            return handler;
        }
    }
}