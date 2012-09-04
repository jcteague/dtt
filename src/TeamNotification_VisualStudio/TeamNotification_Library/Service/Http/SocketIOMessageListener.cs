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
            socket.Error += (s, e) =>
                                   {
                                       int a = 0;
                                       Debug.WriteLine(e);
                                   };

//            socket.Message += (s, e) =>
//                                             {
//                                                 int a = 0;
//                                                 Debug.WriteLine(e);
//                                             };

            socket.On("connect", fn =>
            {
                Debug.WriteLine("Connected event");
                Debug.WriteLine(fn);
            });

            socket.On("message", (data) =>
            {
                int a = 0;
                Debug.WriteLine("Here inside the message");
                Debug.WriteLine(data);
            });

            socket.On("mymessage", (data) =>
            {
                int a = 0;
                Debug.WriteLine("Here inside the mymessage");
                Debug.WriteLine(data);
            });

            socket.On("messagesocket", (data) =>
            {
                int a = 0;
                Debug.WriteLine("Here inside the messagesocket");
                Debug.WriteLine(data);
            });

            socket.Connect();
        }

        public Action<string, byte[]> SubscribeResponse
        {
            get { throw new NotImplementedException(); }
        }
    }
}