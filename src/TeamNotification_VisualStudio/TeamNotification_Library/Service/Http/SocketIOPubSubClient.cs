using System;
using System.Collections.Generic;
using System.Diagnostics;
using SocketIOClient;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Service.Factories;

namespace TeamNotification_Library.Service.Http
{
    public class SocketIOPubSubClient : ISubscribeToPubSub<Action<string, string>>
    {
        private readonly ICreateInstances<IWrapSocketIOClient> socketIOClientFactory;
        private Dictionary<string, IWrapSocketIOClient> socketsStorage;

        public SocketIOPubSubClient(ICreateInstances<IWrapSocketIOClient> socketIOClientFactory)
        {
            this.socketIOClientFactory = socketIOClientFactory;
            socketsStorage = new Dictionary<string, IWrapSocketIOClient>();
        }

        public void Subscribe(string channel, Action<string, string> callback)
        {
            CloseSocketIfExists(channel);
            var socketNamespace = "/room/{0}/messages".FormatUsing(channel.Split(' ')[1]);
            var socket = socketIOClientFactory.GetInstance();
            var roomSocket = socket.Connect(socketNamespace);
            roomSocket.On("message", (data) => callback(channel, data.MessageText));
            StoreSocket(channel, socket);
        }

        private void CloseSocketIfExists(string channel)
        {
            if (socketsStorage.ContainsKey(channel))
                socketsStorage[channel].Close();
        }

        private void StoreSocket(string channel, IWrapSocketIOClient socket)
        {
            socketsStorage[channel] = socket;
        }
    }
}