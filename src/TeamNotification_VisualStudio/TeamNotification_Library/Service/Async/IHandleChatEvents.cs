using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Async
{
    public interface IHandleChatEvents
    {
        event CustomEventHandler<SendMessageWasRequested> SendMessageRequested;

        void OnSendMessageRequested(object source, SendMessageWasRequested eventArgs);

        void Clear();
    }
}