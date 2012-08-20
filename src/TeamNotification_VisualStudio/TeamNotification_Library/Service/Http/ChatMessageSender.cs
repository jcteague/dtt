using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Async;
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

        public void SendMessages(IEnumerable<Block> blocks, string roomId)
        {
            var messages = new List<Tuple<string, HttpContent>>();
            foreach (var block in blocks)
            {
                if (block.GetType() == typeof(BlockUIContainer))
                {
                    var resources = block.Resources;
                    var data = new CodeClipboardData
                    {
                        message = resources["message"].Cast<string>(),
                        solution = resources["solution"].Cast<string>(),
                        document = resources["document"].Cast<string>(),
                        line = resources["line"].Cast<int>(),
                        column = resources["column"].Cast<int>()
                    };
                    messages.Add(GetMessage(data, roomId));
                }
                else
                {
                    var data = new PlainClipboardData { message = ((Paragraph)block).GetText() };
                    messages.Add(GetMessage(data, roomId));
                }    
            }

            client.Post(messages);
        }

        private Tuple<string, HttpContent> GetMessage<T>(T data, string roomId) where T : ChatMessageData
        {
            var url = "{0}room/{1}/messages".FormatUsing(serverConfiguration.Get().Uri, roomId);
            return new Tuple<string, HttpContent>(url, objectToFormMapper.MapFrom(data));
        }
    }
}