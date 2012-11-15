using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Async
{
    public class ChatEvents : AbstractEventHandler, IHandleChatEvents
    {
        public event CustomEventHandler<SendMessageWasRequested> SendMessageRequested;

        public void OnSendMessageRequested(object source, SendMessageWasRequested eventArgs)
        {
            Handle(source, SendMessageRequested, eventArgs);
        }

        public void Clear()
        {
            SendMessageRequested = null;
        }
    }
}