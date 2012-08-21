using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Documents;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Mappers;
using TeamNotification_Library.Extensions;

namespace TeamNotification_Library.Service.Http
{
    public class ChatMessageSender : ISendChatMessages
    {
        readonly ISendHttpRequests client;
        readonly IProvideConfiguration<ServerConfiguration> serverConfiguration;
        readonly IMapPropertiesToFormUrlEncodedContent objectToFormMapper;

        public ChatMessageSender(ISendHttpRequests client, IProvideConfiguration<ServerConfiguration> serverConfiguration, IMapPropertiesToFormUrlEncodedContent objectToFormMapper)
        {
            this.client = client;
            this.serverConfiguration = serverConfiguration;
            this.objectToFormMapper = objectToFormMapper;
        }

        public void SendMessage(Block block, string roomId)
        {
            if (block.GetType() == typeof(BlockUIContainer))
            {
                var resources = block.Resources;
                var data = new CodeClipboardData
                               {
                                   project = resources["project"].Cast<string>(),
                                   message = resources["message"].Cast<string>(),
                                   solution = resources["solution"].Cast<string>(),
                                   document = resources["document"].Cast<string>(),
                                   line = resources["line"].Cast<int>()
                               };
                SendMessage(data, roomId);
            }
            else
            {
                var data = new PlainClipboardData { message = ((Paragraph)block).GetText() };
                SendMessage(data, roomId);
            }
        }

        private void SendMessage<T>(T message, string roomId) where T : ChatMessageData
        {           
            var url = "{0}room/{1}/messages".FormatUsing(serverConfiguration.Get().Uri, roomId);
            client.Post<ServerResponse>(url, objectToFormMapper.MapFrom(message));
        }
    }
}