using System;

namespace TeamNotification_Library.Service.Http
{
    public interface ISubscribeToPubSub<T>
    {
        void Subscribe(string channel, T callback, Action reconnectCallback = null);
    }
}