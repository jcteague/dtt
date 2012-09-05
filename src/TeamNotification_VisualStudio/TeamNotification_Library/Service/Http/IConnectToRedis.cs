using System;

namespace TeamNotification_Library.Service.Http
{
    public interface IConnectToRedis
    {
        void Subscribe(string channel, Action<string,byte[]> callback);
    }
}