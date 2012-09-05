using System;
using System.Diagnostics;
using SocketIOClient;

namespace TeamNotification_Library.Service.Http
{
    public class SocketIOMessageListener : IListenToMessages
    {
        private Client socket;

        public SocketIOMessageListener()
        {
            socket = new Client(Properties.Settings.Default.Site);
        }

        public void ListenOnChannel(string channel, MessageReceivedAction action)
        {
            socket.Message += OnSocketMessage;

            socket.On("messagesocket", (data) =>
            {
                int a = 0;
                Debug.WriteLine(data);
            });

            socket.Connect();
        }

        private void OnSocketMessage(object sender, MessageEventArgs e)
        {
            int a = 0;
            Debug.WriteLine(e);
        }

        public Action<string, byte[]> SubscribeResponse
        {
            get { throw new NotImplementedException(); }
        }
    }
}