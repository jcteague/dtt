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
        private IProvideConfiguration<ServerConfiguration> configuration;

        public ChatRoomsControlService(IProvideUser userProvider, ISendHttpRequests httpClient, IProvideConfiguration<ServerConfiguration> configuration)
        {
            this.userProvider = userProvider;
            this.httpClient = httpClient;
            this.configuration = configuration;
        }

        public Collection GetMessagesCollection(string roomId)
        {
            var user = userProvider.GetUser();
            var uri = configuration.Get().HREF + "room/" + roomId + "/messages";
            var c = httpClient.Get<Collection>(uri).Result;
            return c;
        }

        public Collection GetCollection()
        {
            var user = userProvider.GetUser();
            var uri = configuration.Get().HREF +"user/"+ user.id;
            var c = httpClient.Get<Collection>(uri).Result;
            return c;
        }

    }
}