using System;

namespace TeamNotification_Library.Service.Http
{
    public interface ISubscribeToPubSub<T>
    {
        void UnSubscribe(string channel);
        void Subscribe(string channel, T callback, Action reconnectCallback = null, Action onConnectCallback = null);
    }
}