using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows.Documents;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.Async.Models;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.Mappers;

namespace TeamNotification_Library.Service.Chat
{
    public class ChatMessageSender : ISendChatMessages
    {
        readonly ISendHttpRequests client;
        readonly IProvideConfiguration<ServerConfiguration> serverConfiguration;
        readonly IMapPropertiesToFormUrlEncodedContent objectToFormMapper;
        private IHandleChatEvents chatEvents;

        public ChatMessageSender(ISendHttpRequests client, IProvideConfiguration<ServerConfiguration> serverConfiguration, IMapPropertiesToFormUrlEncodedContent objectToFormMapper, IHandleChatEvents chatEvents)
        {
            this.client = client;
            this.serverConfiguration = serverConfiguration;
            this.objectToFormMapper = objectToFormMapper;
            this.chatEvents = chatEvents;
        }

        public void Initialize()
        {
            chatEvents.SendMessageRequested += SendMessage;
        }

        private void SendMessage(object sender, SendMessageWasRequested e)
        {
            //var url = "{0}room/{1}/messages".FormatUsing(serverConfiguration.Get().Uri, e.RoomId);
            foreach(var message in e.Messages)
            {
                var messages = new List<Tuple<string, HttpContent>> {GetMessage(message, e.RoomId)};

                client.Post(messages);
            }
        }

        public void SendMessage(ChatMessageBody editedMessage, string roomId)
        {
            var messages = new List<Tuple<string, HttpContent>> {GetMessage(editedMessage, roomId)};
            AppendPlainMessage(messages, "", roomId);
            client.Post(messages);
        }

        public void SendMessages(IEnumerable<ChatMessageBody> messages, string roomId)
        {
            var messageBodies = new List<Tuple<string, HttpContent>>();
            foreach (var message in messages)
                messageBodies.Add(GetMessage(message,roomId));
            client.Post(messageBodies);
        }

        private void AppendPlainMessage(List<Tuple<string, HttpContent>> messages, string plainMessage, string roomId)
        {
            if (plainMessage.IsNullOrWhiteSpace()) return;
            var plainData = new ChatMessageBody{message = plainMessage };
            messages.Add(GetMessage(plainData, roomId));
        }

        private Tuple<string, HttpContent> GetMessage(ChatMessageBody data, string roomId)
        {
            var url = "{0}room/{1}/messages".FormatUsing(serverConfiguration.Get().Uri, roomId);
            return new Tuple<string, HttpContent>(url, objectToFormMapper.MapFrom(data));
        }
    }
}
