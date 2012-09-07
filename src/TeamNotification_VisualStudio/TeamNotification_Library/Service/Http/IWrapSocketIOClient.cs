using System;
using SocketIOClient;
using SocketIOClient.Messages;

namespace TeamNotification_Library.Service.Http
{
    public interface IWrapSocketIOClient
    {
        IEndPointClient Connect(string socketNamespace);
        void On(string eventName, Action<IMessage> action);
        void On(string eventName, string endPoint, Action<IMessage> action);
    }
}