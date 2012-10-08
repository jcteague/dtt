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
using TeamNotification_Library.Service.Logging;
using TeamNotification_Library.Service.Mappers;
using System.Linq;
using TeamNotification_Library.Extensions;

namespace TeamNotification_Library.Service.Controls
{
    public class LoginControlService : IServiceLoginControl
    {
        private readonly ISendHttpRequests httpClient;
        private readonly IProvideConfiguration<LoginConfiguration> configuration;
        private readonly IMapEntities<IEnumerable<CollectionData>, FormUrlEncodedContent> mapper;
        private readonly IStoreDataLocally localStorageService;
        private readonly IHandleUserAccountEvents userAccountEvents;
        private ILog logger;

        public LoginControlService(ISendHttpRequests httpClient, IProvideConfiguration<LoginConfiguration> configuration, IMapEntities<IEnumerable<CollectionData>, FormUrlEncodedContent> mapper, IStoreDataLocally localStorageService, IHandleUserAccountEvents userAccountEvents, ILog logger)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
            this.mapper = mapper;
            this.localStorageService = localStorageService;
            this.userAccountEvents = userAccountEvents;
            this.logger = logger;
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
                foreach (var item in itemsList)
                    if(item.type == GlobalConstants.Fields.Password)
                    {
                        loginResponse.user.password = item.value;
                        break;
                    }
                localStorageService.Store(loginResponse);
                userAccountEvents.OnLoginSuccess(this, new UserHasLogged(loginResponse.user, loginResponse.redis));
                logger.Info(this, "User has logged: {0}".FormatUsing(loginResponse.user.email));
            }
            else
            {
                userAccountEvents.OnLoginFail(this);
                logger.Info(this, "User could not log in");
            }
        }

        public bool IsUserLogged()
        {
            return localStorageService.GetUser() != null;
        }
    }
}