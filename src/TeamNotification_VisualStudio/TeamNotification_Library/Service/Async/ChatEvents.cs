using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Async
{
    public class ChatEvents : AbstractEventHandler, IHandleChatEvents
    {
        public event CustomEventHandler<SendMessageRequestedWasRequested> SendMessageRequested;

        public void OnSendMessageRequested(object source, SendMessageRequestedWasRequested eventArgs)
        {
            Handle(source, SendMessageRequested, eventArgs);
        }

        public void Clear()
        {
            SendMessageRequested = null;
        }
    }
}