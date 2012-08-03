using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows.Controls;
using System.Windows.Data;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Async;
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

        public event CustomEventHandler UserHasLogged;
        public event CustomEventHandler UserCouldNotLogIn;

        public LoginControlService(ISendHttpRequests httpClient, IProvideConfiguration<LoginConfiguration> configuration, IMapEntities<IEnumerable<CollectionData>, FormUrlEncodedContent> mapper, IStoreDataLocally localStorageService)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
            this.mapper = mapper;
            this.localStorageService = localStorageService;
        }

        public Collection GetCollection()
        {
            return httpClient.Get<Collection>(configuration.Get().HREF).Result;
        }

        public void HandleClick(IEnumerable<CollectionData> items)
        {
            var itemsList = items.ToList();
            var content = mapper.MapFrom(itemsList);
            var task = httpClient.Post<LoginResponse>(configuration.Get().HREF, content);
            
            var loginResponse = task.Result;
            if (loginResponse.success)
            {
                localStorageService.Store(loginResponse.user, itemsList);
                OnLoginSuccess();
            }
            else
            {
                OnLoginFail();
            }
        }

        private void OnLoginSuccess()
        {
            HandleEvent(UserHasLogged);
        }

        private void OnLoginFail()
        {
            HandleEvent(UserCouldNotLogIn);
        }

        private void HandleEvent(CustomEventHandler handler)
        {
            if(handler != null)
                handler(this, new CustomEventArgs());
        }

        public bool IsUserLogged()
        {
            return localStorageService.GetUser() != null;
        }
    }
}