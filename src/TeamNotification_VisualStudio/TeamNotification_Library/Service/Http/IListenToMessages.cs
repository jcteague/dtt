using System;
using System.Windows.Controls;
using SocketIOClient.Messages;

namespace TeamNotification_Library.Service.Http
{
    public interface IListenToMessages<T>
    {
        void ListenOnChannel(string channel, MessageReceivedAction action, Action reconnectCallback, Action onConnectCallback );
        T SubscribeResponse { get; }
    }
}