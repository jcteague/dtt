using System;
using System.Diagnostics;
using SocketIOClient;

namespace TeamNotification_Library.Service.Http
{
    public class SocketIOMessageListener : IListenToMessages<Action<string, string>>
    {
        readonly ISubscribeToPubSub<Action<string, string>> client;

        public SocketIOMessageListener(ISubscribeToPubSub<Action<string, string>> client)
        {
            this.client = client;
        }

        public void ListenOnChannel(string channel, MessageReceivedAction action)
        {
            SubscribeResponse = (c, payload) => action(c, payload);
            client.Subscribe(channel, SubscribeResponse);
        }

        public Action<string, string> SubscribeResponse { get; private set; }
    }
}