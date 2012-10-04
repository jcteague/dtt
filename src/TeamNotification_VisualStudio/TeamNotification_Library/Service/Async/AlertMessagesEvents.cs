using System;

namespace TeamNotification_Library.Service.Async
{
    public class AlertMessagesEvents : AbstractEventHandler, IHandleAlertMessages
    {
        public event CustomEventHandler<AlertMessageWasRequested> AlertMessageWasRequested;

        public void OnAlertMessageRequested(object source, AlertMessageWasRequested eventArgs)
        {
            Handle(source, AlertMessageWasRequested, eventArgs);
        }

        public void Clear()
        {
            AlertMessageWasRequested = null;
        }
    }
}