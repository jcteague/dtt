using System;
using SocketIOClient;
using TeamNotification_Library.Service.Http;

namespace TeamNotification_Library.Service.Factories
{
    public class SocketIOClientFactory : ICreateInstances<IWrapSocketIOClient>
    {
        public IWrapSocketIOClient GetInstance()
        {
            var socket = new SocketIOClientWrapper(Properties.Settings.Default.Site) {RetryConnectionAttempts = 5};
            socket.Connect();

            return socket;
        }
    }
}