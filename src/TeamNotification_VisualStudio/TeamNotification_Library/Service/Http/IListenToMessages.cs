using System;

namespace TeamNotification_Library.Service.Http
{
    public interface IListenToMessages<T>
    {
        void ListenOnChannel(string channel,MessageReceivedAction action);
        T SubscribeResponse { get; }
    }
}