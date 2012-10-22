using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Async
{
    public interface IHandleSocketIOEvents
    {
        event CustomEventHandler<SocketWasDisconnected> SocketWasDisconnected;

        void OnSocketWasDisconnected(object source, SocketWasDisconnected args);

        void Clear();
    }
}