using System.Net;
using System.Net.Http;
using Machine.Specifications;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Factories;
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
            };

            protected static IProvideUser userProvider;
            protected static ICreateInstances<HttpClientHandler> httpClientHandlerFactory;
        }

        public abstract class when_getting_the_handler : Concern
        {
            Establish context = () =>
            {
                httpClientHandler = fake.an<HttpClientHandler>();
                httpClientHandlerFactory.Stub(x => x.GetInstance()).Return(httpClientHandler);
            };

            protected static HttpClientHandler httpClientHandler;
        }

        public class when_getting_the_handler_and_the_user_provider_returns_a_user : when_getting_the_handler
        {
            Establish context = () =>
            {
                user = new User
                           {
                               email = "foo@bar.com",
                               password = "blah"
                           };
                userProvider.Stub(x => x.GetUser()).Return(user);
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

            private static User user;
            private static HttpClientHandler result;
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