using System;
using System.Diagnostics;
using SocketIOClient;

namespace TeamNotification_Library.Service.Http
{
    public class SocketIOMessageListener : IListenToMessages
    {
        private Client socket;
        readonly ISubscribeToPubSub<Action<string, string>> client;

        public SocketIOMessageListener(ISubscribeToPubSub<Action<string, string>> client)
        {
            this.client = client;
        }

        public void ListenOnChannel(string channel, MessageReceivedAction action)
        {
            client.Subscribe(channel, (c, payload) => action(c, payload));
        }

        public Action<string, byte[]> SubscribeResponse
        {
            get { throw new NotImplementedException(); }
        }
    }
}