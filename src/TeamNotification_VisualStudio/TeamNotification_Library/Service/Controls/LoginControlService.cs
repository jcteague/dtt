using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows.Controls;
using System.Windows.Data;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.Async.Models;
using TeamNotification_Library.Service.Content;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.Mappers;
using System.Linq;

namespace TeamNotification_Library.Service.Controls
{
    public class LoginControlService : IServiceLoginControl
    {
        private readonly ISendHttpRequests httpClient;
        private readonly IProvideConfiguration<LoginConfiguration> configuration;
        private readonly IMapEntities<IEnumerable<CollectionData>, FormUrlEncodedContent> mapper;
        private readonly IStoreDataLocally localStorageService;
        private readonly IHandleLoginEvents loginEvents;

        public LoginControlService(ISendHttpRequests httpClient, IProvideConfiguration<LoginConfiguration> configuration, IMapEntities<IEnumerable<CollectionData>, FormUrlEncodedContent> mapper, IStoreDataLocally localStorageService, IHandleLoginEvents loginEvents)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
            this.mapper = mapper;
            this.localStorageService = localStorageService;
            this.loginEvents = loginEvents;
        }

        public Collection GetCollection()
        {
            return httpClient.Get<Collection>(configuration.Get().Uri).Result;
        }

        public void HandleClick(IEnumerable<CollectionData> items)
        {
            var itemsList = items.ToList();
            var content = mapper.MapFrom(itemsList);
            var task = httpClient.Post<LoginResponse>(configuration.Get().Uri, content);
            var loginResponse = task.Result;
            if (loginResponse.success)
            {
                foreach (var item in items)
                    if(item.type == "password")
                    {
                        loginResponse.user.password = item.value;
                        break;
                    }
                localStorageService.Store(loginResponse, itemsList);
                loginEvents.OnLoginSuccess(this, new UserHasLogged(loginResponse.user, loginResponse.redis));
            }
            else
            {
                loginEvents.OnLoginFail(this);
            }
        }

        public bool IsUserLogged()
        {
            return localStorageService.GetUser() != null;
        }
    }
}