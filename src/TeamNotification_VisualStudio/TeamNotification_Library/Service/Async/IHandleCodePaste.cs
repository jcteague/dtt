using System;
using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Async
{
    public interface IHandleCodePaste
    {
        event CustomEventHandler<EventArgs> CodePasteWasClicked;

        void OnCodePasteClick(object source, EventArgs eventArgs);
    }
}