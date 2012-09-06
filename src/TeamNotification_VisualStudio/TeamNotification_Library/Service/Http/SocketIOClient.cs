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
//            socket.Message += OnSocketMessage;
            var socketNamespace = "/room/{0}/messages".FormatUsing(channel.Split(' ')[1]);

            var roomSocket = socket.Connect(socketNamespace);
            roomSocket.On("messagesocket", (data) =>
            {
                callback(channel, data.Json.Args[0]);
            });

//            socket.On("messagesocket", socketNamespace, (data) =>
//            {
//                callback(channel, data.Json.Args[0]);
//            });
//
//            socket.Connect(socketNamespace);
        }

        private void OnSocketMessage(object sender, MessageEventArgs e)
        {
            int a = 0;
            Debug.WriteLine(e);
        }
    }
}