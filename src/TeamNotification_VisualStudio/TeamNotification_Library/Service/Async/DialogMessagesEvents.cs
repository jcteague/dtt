using System;

namespace TeamNotification_Library.Service.Async
{
    public class DialogMessagesEvents : AbstractEventHandler, IHandleDialogMessages
    {
        public event CustomEventHandler<AlertMessageWasRequested> AlertMessageWasRequested;
        public event CustomEventHandler<DialogMessageWasRequested> DialogMessageWasRequested;

        public void OnAlertMessageRequested(object source, AlertMessageWasRequested eventArgs)
        {
            Handle(source, AlertMessageWasRequested, eventArgs);
        }

        public void OnDialogMessageRequested(object source, DialogMessageWasRequested eventArgs)
        {
            Handle(source, DialogMessageWasRequested, eventArgs);
        }

        public void Clear()
        {
            AlertMessageWasRequested = null;
            DialogMessageWasRequested = null;
        }
    }
}