using System;
using System.Diagnostics;
using SocketIOClient;
using TeamNotification_Library.Extensions;

namespace TeamNotification_Library.Service.Http
{
    public class SocketIOClient : ISubscribeToPubSub<Action<string, string>>
    {
        private Client socket;

        public SocketIOClient()
        {
            socket = new Client(Properties.Settings.Default.Site);
            socket.Connect();
        }

        public void Subscribe(string channel, Action<string, string> callback)
        {
            var socketNamespace = "/room/{0}/messages".FormatUsing(channel.Split(' ')[1]);
            var roomSocket = socket.Connect(socketNamespace);
            roomSocket.On("message", (data) =>
            {
                callback(channel, data.MessageText);
            });

        }
    }
}