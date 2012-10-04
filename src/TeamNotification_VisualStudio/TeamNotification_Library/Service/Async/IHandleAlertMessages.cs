using System;

namespace TeamNotification_Library.Service.Async
{
    public interface IHandleAlertMessages
    {
        event CustomEventHandler<AlertMessageWasRequested> AlertMessageWasRequested;
        void OnAlertMessageRequested(object source, AlertMessageWasRequested eventArgs);
        void Clear();
    }
}