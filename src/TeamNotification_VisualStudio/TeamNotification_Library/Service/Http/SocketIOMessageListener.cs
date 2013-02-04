using System;
using System.Collections.Generic;
using System.Diagnostics;
using SocketIOClient;
using SocketIOClient.Messages;

namespace TeamNotification_Library.Service.Http
{
    public class SocketIOMessageListener : IListenToMessages<Action<string, string>>
    {
        readonly ISubscribeToPubSub<Action<string, string>> client;

        public SocketIOMessageListener(ISubscribeToPubSub<Action<string, string>> client)
        {
            this.client = client;
        }

        public void ListenOnChannel(string channel, MessageReceivedAction action, Action reconnectCallback, Action onConnectCallback)
        {
//            SubscribeResponse = (c, payload) => action(c, payload);
//            client.Subscribe(channel, SubscribeResponse, reconnectCallback, onConnectCallback);
            client.Subscribe(channel, (c, payload) => action(c, payload), reconnectCallback, onConnectCallback);
        }

        public Action<string, string> SubscribeResponse { get; private set; }
    }
}