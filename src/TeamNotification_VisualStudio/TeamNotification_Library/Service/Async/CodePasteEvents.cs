using System;
using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Async
{
    public class CodePasteEvents : AbstractEventHandler, IHandleCodePaste
    {
        public event CustomEventHandler<EventArgs> CodePasteWasClicked;
        public event CustomEventHandler<EventArgs> CodeQuotedWasClicked;

        public void OnCodePasteClick(object source, EventArgs eventArgs)
        {
            Handle(source, CodePasteWasClicked, eventArgs);
        }

        public void OnCodeQuotedClick(object source, EventArgs eventArgs)
        {
            Handle(source, CodeQuotedWasClicked, eventArgs);
        }

        public void Clear()
        {
            CodePasteWasClicked = null;
            CodeQuotedWasClicked = null;
        }
    }
}