using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Async
{
    public interface IHandleChatEvents
    {
        event CustomEventHandler<SendMessageRequestedWasRequested> SendMessageRequested;

        void OnSendMessageRequested(object source, SendMessageRequestedWasRequested eventArgs);

        void Clear();
    }
}