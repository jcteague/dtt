using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Documents;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Http
{
    public class ChatMessageSender : ISendChatMessages
    {
        readonly ISendHttpRequests client;
        readonly IProvideConfiguration<ServerConfiguration> serverConfiguration;
        public ChatMessageSender(ISendHttpRequests client, IProvideConfiguration<ServerConfiguration> serverConfiguration)
        {
            this.client = client;
            this.serverConfiguration = serverConfiguration;
        }

        public void SendMessage<T>(T message, string roomId) where T : ChatMessageData
        {           
            var sb = new StringBuilder(serverConfiguration.Get().Uri);
            var postValues = new List<KeyValuePair<string, string>>();

            sb.AppendFormat("room/{0}/messages", roomId);
            postValues.Add(new KeyValuePair<string, string>("message", message.message));

            client.Post(sb.ToString(), postValues.ToArray());
        }
    }
}