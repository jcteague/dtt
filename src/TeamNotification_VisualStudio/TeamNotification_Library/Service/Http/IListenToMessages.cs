using System;

namespace TeamNotification_Library.Service.Http
{
    public interface IListenToMessages
    {
        void ListenOnChannel(string channel,MessageReceivedAction action);
        Action<string, byte[]> SubscribeResponse { get; }
    }
}