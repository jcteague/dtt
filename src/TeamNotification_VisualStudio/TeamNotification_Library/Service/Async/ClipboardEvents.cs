using EnvDTE;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Async
{
    public class ClipboardEvents : AbstractEventHandler, IHandleClipboardEvents
    {
        public event CustomEventHandler<ClipboardHasChanged> ClipboardHasChanged;

        public void OnClipboardChanged(object source, ClipboardHasChanged args)
        {
            Handle(source, ClipboardHasChanged, args);
        }
    }
}