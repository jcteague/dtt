using System;
using System.Collections.Generic;
using System.Diagnostics;
using SocketIOClient;
using SocketIOClient.Messages;
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

        public void Subscribe(string channel, Action<string, string> messageCallback, Action reconnectCallback)
        {
            CloseSocketIfExists(channel);
            var socketNamespace = "/room/{0}/messages".FormatUsing(channel.Split(' ')[1]);
            var socket = socketIOClientFactory.GetInstance();
            var roomSocket = socket.Connect(socketNamespace, reconnectCallback);
            roomSocket.On("message", (data) => messageCallback(channel, data.MessageText));
            //roomSocket.On("reconnect", reconnectCallback);
            //roomSocket.On("retry", this.someKewlMethod);
            StoreSocket(channel, socket);
        }

        private void someKewlMethod(object arg)
        {
            var x = 5;
            Debug.WriteLine("Yay!");
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