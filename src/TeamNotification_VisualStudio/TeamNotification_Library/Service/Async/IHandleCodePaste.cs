using System;
using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Async
{
    public interface IHandleCodePaste
    {
        event CustomEventHandler<EventArgs> CodePasteWasClicked;
        event CustomEventHandler<EventArgs> CodeQuotedWasClicked; 
        void OnCodePasteClick(object source, EventArgs eventArgs);
        void OnCodeQuotedClick(object source, EventArgs eventArgs);
        void Clear();
    }
}