using System;
using System.Collections.Generic;
using System.Diagnostics;
using SocketIOClient;

namespace TeamNotification_Library.Service.Http
{
    public class SocketIOMessageListener : IListenToMessages<Action<string, string>>
    {
        readonly ISubscribeToPubSub<Action<string, string>> client;
        private List<string> subscribedChannels;

        public SocketIOMessageListener(ISubscribeToPubSub<Action<string, string>> client)
        {
            this.client = client;
            subscribedChannels = new List<string>();
        }

        public void ListenOnChannel(string channel, MessageReceivedAction action)
        {
            if (IsSubscribedTo(channel)) return;
            SubscribeResponse = (c, payload) => action(c, payload);
            client.Subscribe(channel, SubscribeResponse);
            subscribedChannels.Add(channel);
        }

        public Action<string, string> SubscribeResponse { get; private set; }

        public bool IsSubscribedTo(string channel)
        {
            return subscribedChannels.Contains(channel);
        }

        public void ResetSubscriptionFor(string channel)
        {
            subscribedChannels.Remove(channel);
        }
    }
}