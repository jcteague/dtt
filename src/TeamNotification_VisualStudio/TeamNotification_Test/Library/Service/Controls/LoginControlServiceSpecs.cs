﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Machine.Specifications;
using Rhino.Mocks;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.Async.Models;
using TeamNotification_Library.Service.Controls;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.Mappers;
using developwithpassion.specifications.rhinomocks;

namespace TeamNotification_Test.Library.Service.Controls
{
    [Subject(typeof(LoginControlService))]
    public class LoginControlServiceSpecs
    {
        public abstract class Concern : Observes<IServiceLoginControl, LoginControlService>
        {
            Establish context = () =>
            {
                httpClient = depends.on<ISendHttpRequests>();
                configurationProvider = depends.on<IProvideConfiguration<LoginConfiguration>>();                        
                collectionToUrlEncodedFormMapper = depends.on<IMapEntities<IEnumerable<CollectionData>, FormUrlEncodedContent>>();
                localStorageService = depends.on<IStoreDataLocally>();
                userAccountEvents = depends.on<IHandleUserAccountEvents>();

                configuration = fake.an<IStoreConfiguration>();
                configurationProvider.Stub(x => x.Get()).Return(configuration);

                configuration.Stub(x => x.Uri).Return("blah href");
                configurationProvider.Stub(x => x.Get()).Return(configuration);
            };

            protected static ISendHttpRequests httpClient;
            protected static IProvideConfiguration<LoginConfiguration> configurationProvider;
            protected static IStoreConfiguration configuration;
            protected static IMapEntities<IEnumerable<CollectionData>, FormUrlEncodedContent> collectionToUrlEncodedFormMapper;
            protected static IStoreDataLocally localStorageService;
            protected static IHandleUserAccountEvents userAccountEvents;
        }

        public class when_getting_the_collection : Concern
        {
            Establish context = () =>
            {
                collection = fake.an<Collection>();

                var collectionTask = Task.Factory.StartNew(() => collection);
                httpClient.Stub(x => x.Get<Collection>(configuration.Uri)).Return(collectionTask);
            };

            Because of = () =>
                result = sut.GetCollection();

            It should_return_a_collection_from_the_href = () =>
                result.ShouldEqual(collection);
            
            private static Collection result;
            private static Collection collection;
        }

        public abstract class when_the_submit_button_is_clicked : Concern
        {
            Establish context = () =>
            {
                var collectionData1 = new CollectionData { label = "foo", value = "foo value" };
                var collectionData2 = new CollectionData { label = "bar", value = "bar value" };
                collectionDataList = new List<CollectionData> { collectionData1, collectionData2 };

                var data = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("foo", "fooValue"), new KeyValuePair<string, string>("bar", "barValue") };
                postData = new FormUrlEncodedContent(data);
                collectionToUrlEncodedFormMapper.Stub(x => x.MapFrom(collectionDataList)).Return(postData);
            };

            protected static FormUrlEncodedContent postData;
            protected static List<CollectionData> collectionDataList;
        }

        public class when_the_submit_button_is_clicked_and_the_user_and_password_is_correct : when_the_submit_button_is_clicked
        {
            Establish context = () =>
            {
                user = new User
                {
                    id = 10,
                    email = "foo@bar.com"
                };
                redisConfig = new Collection.RedisConfig
                                  {
                                      host = "dtt.local",
                                      port = "9367"
                                  };
                response = new LoginResponse {redis = redisConfig, success = true, user = user};
                var loginResponseTask = Task.Factory.StartNew(() => response);
                httpClient.Stub(x => x.Post<LoginResponse>(configuration.Uri, postData)).Return(loginResponseTask);
            };

            Because of = () =>
                sut.HandleClick(collectionDataList);

            It should_store_the_response_user_data_locally = () =>
                localStorageService.AssertWasCalled(x => x.Store(response));

            It should_call_the_on_login_success_event = () =>
                userAccountEvents.AssertWasCalled(x => x.OnLoginSuccess(Arg<IServiceLoginControl>.Is.Same(sut), Arg<UserHasLogged>.Matches(y => y.RedisConfig == redisConfig && y.User == user)));

            private static User user;
            private static Collection.RedisConfig redisConfig;
            private static LoginResponse response;
        }

        public class when_the_submit_button_is_clicked_and_the_user_and_password_are_not_correct : when_the_submit_button_is_clicked
        {
            Establish context = () =>
            {
                user = new User
                {
                    id = 10,
                    email = "foo@bar.com"
                };

                var loginResponseTask = Task.Factory.StartNew(() => new LoginResponse { success = false });
                httpClient.Stub(x => x.Post<LoginResponse>(configuration.Uri, postData)).Return(loginResponseTask);
            };

            Because of = () =>
                sut.HandleClick(collectionDataList);

            It should_not_store_the_response_user_data_locally = () =>
                localStorageService.AssertWasNotCalled(x => x.Store(Arg<LoginResponse>.Is.Anything));

            It should_call_the_on_login_fail_event = () =>
                userAccountEvents.AssertWasCalled(x => x.OnLoginFail(sut));

            private static User user;
        }
    }
}