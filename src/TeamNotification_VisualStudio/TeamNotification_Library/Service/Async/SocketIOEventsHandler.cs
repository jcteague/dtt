using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Async
{
    public class SocketIOEventsHandler : AbstractEventHandler, IHandleSocketIOEvents
    {
        public event CustomEventHandler<SocketWasDisconnected> SocketWasDisconnected;

        public void OnSocketWasDisconnected(object source, SocketWasDisconnected args)
        {
            Handle(source, SocketWasDisconnected, args);
        }

        public void Clear()
        {
            SocketWasDisconnected = null;
        }
    }
}