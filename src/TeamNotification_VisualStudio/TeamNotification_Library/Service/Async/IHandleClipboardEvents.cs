namespace TeamNotification_Library.Service.Async
{
    public interface IHandleClipboardEvents
    {
        event CustomEventHandler ClipboardHasChanged;

        void OnClipboardChanged(object source, CustomEventArgs args);
    }
}