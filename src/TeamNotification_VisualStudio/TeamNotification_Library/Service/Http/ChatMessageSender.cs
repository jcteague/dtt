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
using TeamNotification_Library.Service.Logging;
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

        public void SendMessage(ChatMessageBody editedMessage, string roomId)
        {
            var messages = new List<Tuple<string, HttpContent>> {GetMessage(editedMessage, roomId)};

            AppendPlainMessage(messages, "", roomId);
            client.Post(messages);
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
                    var chatMessageBody = new ChatMessageBody
                    {
                        project = resources["project"].Cast<string>(),
                        message = resources["message"].Cast<string>(),
                        solution = resources["solution"].Cast<string>(),
                        document = resources["document"].Cast<string>(),
                        line = resources["line"].Cast<int>(),
                        column = resources["column"].Cast<int>(),
                        programminglanguage = resources["programminglanguage"].Cast<int>(),
                        date = resources["date"].Cast<string>()
                    };
                    messages.Add(GetMessage(chatMessageBody, roomId));
                    plainMessage = "";
                }
                else
                {
                    var text = ((Paragraph) block).GetText();
                    plainMessage = plainMessage.IsNullOrEmpty() ? text : plainMessage + "\r\n" + text;
                }
            }

            AppendPlainMessage(messages, plainMessage, roomId);
            Container.GetInstance<ILog>().Write("Posting: {0}".FormatUsing(plainMessage));
            client.Post(messages);
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