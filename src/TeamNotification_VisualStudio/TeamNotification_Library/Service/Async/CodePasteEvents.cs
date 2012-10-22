using System;
using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Async
{
    public class CodePasteEvents : AbstractEventHandler, IHandleCodePaste
    {
        public event CustomEventHandler<EventArgs> CodePasteWasClicked;

        public void OnCodePasteClick(object source, EventArgs eventArgs)
        {
            Handle(source, CodePasteWasClicked, eventArgs);
        }

        public void Clear()
        {
            CodePasteWasClicked = null;
        }
    }
}