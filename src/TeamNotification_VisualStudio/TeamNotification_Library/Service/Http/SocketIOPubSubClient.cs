using System;
using System.Diagnostics;
using SocketIOClient;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Service.Factories;

namespace TeamNotification_Library.Service.Http
{
    public class SocketIOPubSubClient : ISubscribeToPubSub<Action<string, string>>
    {
        private readonly ICreateInstances<IWrapSocketIOClient> socketIOClientFactory;

        public SocketIOPubSubClient(ICreateInstances<IWrapSocketIOClient> socketIOClientFactory)
        {
            this.socketIOClientFactory = socketIOClientFactory;
        }

        public void Subscribe(string channel, Action<string, string> callback)
        {
            var socketNamespace = "/room/{0}/messages".FormatUsing(channel.Split(' ')[1]);
            var socket = socketIOClientFactory.GetInstance();
            var roomSocket = socket.Connect(socketNamespace);
            roomSocket.On("message", (data) => callback(channel, data.MessageText));
        }
    }
}