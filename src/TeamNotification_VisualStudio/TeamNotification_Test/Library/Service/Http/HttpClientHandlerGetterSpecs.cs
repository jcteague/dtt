using System;
using System.Net;
using System.Net.Http;
using Machine.Specifications;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Factories;
using TeamNotification_Library.Service.FileSystem;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.Providers;
using developwithpassion.specifications.rhinomocks;
using Rhino.Mocks;

namespace TeamNotification_Test.Library.Service.Http
{
    [Subject(typeof(HttpClientHandlerGetter))]
    public class HttpClientHandlerGetterSpecs
    {
        public abstract class Concern : Observes<IGetHttpClientHandler, HttpClientHandlerGetter>
        {
            Establish context = () =>
            {
                userProvider = depends.on<IProvideUser>();
                httpClientHandlerFactory = depends.on<ICreateInstances<HttpClientHandler>>();
                encoder = depends.on<IHandleEncoding>();
                serverConfigurationProvider = depends.on<IProvideConfiguration<ServerConfiguration>>();
            };

            protected static IProvideConfiguration<ServerConfiguration> serverConfigurationProvider;
            protected static IHandleEncoding encoder;
            protected static IProvideUser userProvider;
            protected static ICreateInstances<HttpClientHandler> httpClientHandlerFactory;
        }

        public abstract class when_getting_the_handler : Concern
        {
            Establish context = () =>
            {
                httpClientHandler = fake.an<HttpClientHandler>();
                serverConfiguration = new ServerConfiguration() {Uri = "http://SomeUri:545/"};
                httpClientHandlerFactory.Stub(x => x.GetInstance()).Return(httpClientHandler);
                serverConfigurationProvider.Stub(x => x.Get()).Return(serverConfiguration);
            };

            protected static ServerConfiguration serverConfiguration;
            protected static HttpClientHandler httpClientHandler;
        }

        public class when_getting_the_handler_and_the_user_provider_returns_a_user : when_getting_the_handler
        {
            Establish context = () =>
            {
                cookieName = "authtoken";
                uri = new Uri(serverConfiguration.Uri);
                httpClientHandler.CookieContainer = fake.an<CookieContainer>();
                user = new User
                           {
                               email = "foo@bar.com",
                               password = "blah"
                           };
                userProvider.Stub(x => x.GetUser()).Return(user);
                encoder.Stub(x => x.Encode(user.email + ":" + user.password)).Return("Some text");
            };

            Because of = () =>
                result = sut.GetHandler();

            It should_return_a_handler_with_the_credentials = () =>
            {
                result.Credentials.ShouldNotBeNull();
                result.Credentials.ShouldBeOfType<NetworkCredential>();
            };

            It should_return_the_handler = () =>
                result.ShouldEqual(httpClientHandler);

            It should_return_a_handler_with_a_cookie = () =>
                 httpClientHandler.CookieContainer.Count.ShouldEqual(1);

            It should_return_a_handler_with_a_valid_cookie = () =>
                 httpClientHandler.CookieContainer.GetCookies(uri)[0].Name.ShouldEqual(cookieName);

            private static Uri uri;
            private static User user;
            private static HttpClientHandler result;
            private static string cookieName;
        }

        public class when_getting_the_handler_and_the_user_provider_returns_null : when_getting_the_handler
        {
            Establish context = () =>
            {
                userProvider.Stub(x => x.GetUser()).Return(null);
            };

            Because of = () =>
                result = sut.GetHandler();

            It should_return_a_handler_with_the_credentials = () => 
                result.Credentials.ShouldBeNull();

            It should_return_the_handler = () =>
                result.ShouldEqual(httpClientHandler);

            private static User user;
            private static HttpClientHandler result;
        }
         
    }
}