using System;

namespace TeamNotification_Library.Service.Async
{
    public interface IHandleDialogMessages
    {
        event CustomEventHandler<AlertMessageWasRequested> AlertMessageWasRequested;
        event CustomEventHandler<DialogMessageWasRequested> DialogMessageWasRequested;
        void OnAlertMessageRequested(object source, AlertMessageWasRequested eventArgs);
        void OnDialogMessageRequested(object source, DialogMessageWasRequested eventArgs);
        void Clear();
    }
}