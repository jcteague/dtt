using System;
using System.Text;
using System.Windows.Controls;
using SocketIOClient.Messages;
using TeamNotification_Library.Service.Providers;

namespace TeamNotification_Library.Service.Http
{
    public class MessageListener : IListenToMessages<Action<string, byte[]>>
    {
        readonly ISubscribeToPubSub<Action<string, byte[]>> client;

        private MessageReceivedAction onMessageReceivedAction;
        private Action<string, byte[]> onMessageReceivedActionExcecution;

        public MessageListener(ISubscribeToPubSub<Action<string, byte[]>> client)
        {
            this.client = client;
        }

        public void ListenOnChannel(string channel, MessageReceivedAction action, Action reconnectCallback)
        {
            onMessageReceivedAction = action;
            onMessageReceivedActionExcecution = (c, bytes) => 
                onMessageReceivedAction(c, new UTF8Encoding().GetString(bytes));
            client.Subscribe(channel, SubscribeResponse, reconnectCallback);
        }

        public Action<string, byte[]> SubscribeResponse
        {
            get { return onMessageReceivedActionExcecution; }
        }
    }
}