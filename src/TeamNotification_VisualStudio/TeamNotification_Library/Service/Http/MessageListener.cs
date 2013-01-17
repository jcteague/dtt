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
        private string currentChannel = "";
        private MessageReceivedAction onMessageReceivedAction;
        private Action<string, byte[]> onMessageReceivedActionExcecution;

        public MessageListener(ISubscribeToPubSub<Action<string, byte[]>> client)
        {
            this.client = client;
        }

        public void ListenOnChannel(string channel, MessageReceivedAction action, Action reconnectCallback, Action onConnectCallback)
        {
            onMessageReceivedAction = action;
            onMessageReceivedActionExcecution = (c, bytes) => 
                onMessageReceivedAction(c, new UTF8Encoding().GetString(bytes));
            if (!string.IsNullOrEmpty(currentChannel))
                client.UnSubscribe(channel);
            client.Subscribe(channel, SubscribeResponse, reconnectCallback, onConnectCallback);
            currentChannel = channel;
        }

        public Action<string, byte[]> SubscribeResponse
        {
            get { return onMessageReceivedActionExcecution; }
        }
    }
}