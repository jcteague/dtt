using System.Collections.Generic;
using System.Net.Http;
using System.Windows.Controls;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.Mappers;

namespace TeamNotification_Library.Service.Controls
{
    public class LoginControlService : IServiceLoginControl
    {
        private readonly ISendHttpRequests httpClient;
        private readonly IProvideConfiguration<LoginConfiguration> configuration;
        private readonly IMapEntities<IEnumerable<CollectionData>, FormUrlEncodedContent> mapper;
        private readonly IStoreDataLocally localStorageService;

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
            var content = mapper.MapFrom(items);
            httpClient.Post<LoginResponse>(configuration.Get().HREF, content)
                .ContinueWith(x =>
                                  {
                                      if (x.Result.success) localStorageService.Store(x.Result.user);
                                  });
        }

        public bool IsUserLogged()
        {
            return localStorageService.User != null;
        }
    }
}