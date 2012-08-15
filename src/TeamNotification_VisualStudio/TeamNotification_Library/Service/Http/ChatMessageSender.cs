using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        readonly IHandleTasksQueue tasksQueueHandler;

        public ChatMessageSender(ISendHttpRequests client, IProvideConfiguration<ServerConfiguration> serverConfiguration, IMapPropertiesToFormUrlEncodedContent objectToFormMapper, IHandleTasksQueue tasksQueueHandler)
        {
            this.client = client;
            this.serverConfiguration = serverConfiguration;
            this.objectToFormMapper = objectToFormMapper;
            this.tasksQueueHandler = tasksQueueHandler;
        }

        private Queue<Action> actionsQueue = new Queue<Action>();

        public void SendMessages(IEnumerable<Block> blocks, string roomId)
        {
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
                    actionsQueue.Enqueue(() => SendMessage(data, roomId));
                }
                else
                {
                    var data = new PlainClipboardData { message = ((Paragraph)block).GetText() };
                    actionsQueue.Enqueue(() => SendMessage(data, roomId));
                }
            }

            tasksQueueHandler.Enqueue(actionsQueue);
        }
 
        public void SendMessage(Block block, string roomId)
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
            client.PostSync<ServerResponse>(url, objectToFormMapper.MapFrom(message));
        }
    }
}