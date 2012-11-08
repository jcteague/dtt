using System;
using System.Timers;
using SocketIOClient;
using SocketIOClient.Messages;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Http
{
    public class SocketIOClientWrapper : Client, IWrapSocketIOClient
    {
        private IHandleDialogMessages dialogMessageEvents;
        private IHandleSocketIOEvents socketIOEvents;

        public SocketIOClientWrapper(string url) : base(url) 
        {
            dialogMessageEvents = Container.GetInstance<IHandleDialogMessages>();
            socketIOEvents = Container.GetInstance<IHandleSocketIOEvents>();
        }

        public override void On(string eventName, Action<IMessage> action)
        {
            base.On(eventName, action);
        }

        public override void On(string eventName, string endPoint, Action<IMessage> action)
        {
            base.On(eventName, endPoint, action);
        }

        public new IEndPointClient Connect(string socketNamespace, Action reconnectCallback = null, Action onConnectCallback=null )
        {
            SocketConnectionClosed += (s, e) =>
                                               {
                                                   var alertMessage = new AlertMessageWasRequested { Message = "Socket Connection Lost" };
                                                   dialogMessageEvents.OnAlertMessageRequested(this, alertMessage);

                                                   var roomId = socketNamespace.Split('/')[2];
                                                   socketIOEvents.OnSocketWasDisconnected(this, new SocketWasDisconnected { RoomId = roomId });
                                               };

            if (onConnectCallback != null)
                On("connect", (im) => onConnectCallback());
            if (reconnectCallback != null)
                ConnectionRetryAttempt += (o, s) => reconnectCallback();

            return base.Connect("/api" + socketNamespace);
        }
    }
}