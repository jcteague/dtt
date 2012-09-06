using System;
using SocketIOClient;
using SocketIOClient.Messages;

namespace TeamNotification_Library.Service.Http
{
    public class SocketIOClientWrapper : Client, IWrapSocketIOClient
    {
        public SocketIOClientWrapper(string url) : base(url) 
        {
        }

        public override void On(string eventName, Action<IMessage> action)
        {
            base.On(eventName, action);
        }

        public override void On(string eventName, string endPoint, Action<IMessage> action)
        {
            base.On(eventName, endPoint, action);
        }

        public new IEndPointClient Connect(string socketNamespace)
        {
            return base.Connect(socketNamespace);
        }
    }
}