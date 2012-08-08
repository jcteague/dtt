using EnvDTE;
using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Async
{
    public interface IHandleClipboardEvents
    {
        event CustomEventHandler<ClipboardHasChanged> ClipboardHasChanged;

        void Raise(DTE dte);

        void OnClipboardChanged(object source, ClipboardHasChanged args);
    }
}