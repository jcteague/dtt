using System.Collections.Concurrent;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.Providers;
using System.Collections.Generic;

namespace TeamNotification_Library.Service.Controls
{
    public class ChatRoomsControlService : IServiceChatRoomsControl
    {
        private IProvideUser userProvider;
        private ISendHttpRequests httpClient;
        private IProvideConfiguration<LoginConfiguration> configuration;
        public ChatRoomsControlService(IProvideUser userProvider, ISendHttpRequests httpClient, IProvideConfiguration<LoginConfiguration> configuration)
        {
            this.userProvider = userProvider;
            this.httpClient = httpClient;
            this.configuration = configuration;
        }

        public Collection GetCollection()
        {
            var user = userProvider.GetUser();
            var uri = "http://dtt.local:3000/user/" + user.id + "/rooms";
            var c = httpClient.Get<Collection>(uri).Result;

            return c;
        }

    }
}