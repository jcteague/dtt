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
using System.Linq;

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
            var plainMessage = "";
            var messages = new List<Tuple<string, HttpContent>>();
            foreach (var block in blocks)
            {
                if (block.GetType() == typeof(BlockUIContainer))
                {
                    AppendPlainMessage(messages, plainMessage, roomId);

                    var resources = block.Resources;
                    var data = new CodeClipboardData
                    {
                        message = resources["message"].Cast<string>(),
                        solution = resources["solution"].Cast<string>(),
                        document = resources["document"].Cast<string>(),
                        line = resources["line"].Cast<int>(),
                        column = resources["column"].Cast<int>(),
                        programmingLanguage = resources["programmingLanguage"].Cast<int>()
                    };
                    messages.Add(GetMessage(data, roomId));
                    plainMessage = "";
                }
                else
                {
                    plainMessage = plainMessage + "\r\n" + ((Paragraph) block).GetText();
                }    
            }

            AppendPlainMessage(messages, plainMessage, roomId);

            client.Post(messages);
        }

        private void AppendPlainMessage(List<Tuple<string, HttpContent>> messages, string plainMessage, string roomId)
        {
            if (!plainMessage.IsNullOrWhiteSpace())
            {
                var plainData = new PlainClipboardData { message = plainMessage };
                messages.Add(GetMessage(plainData, roomId));
            }
        }

        private Tuple<string, HttpContent> GetMessage<T>(T data, string roomId) where T : ChatMessageData
        {
            var url = "{0}room/{1}/messages".FormatUsing(serverConfiguration.Get().Uri, roomId);
            return new Tuple<string, HttpContent>(url, objectToFormMapper.MapFrom(data));
        }
    }
}