namespace TeamNotification_Library.Service.Async
{
    public class ClipboardEvents : AbstractEventHandler, IHandleClipboardEvents
    {
        public event CustomEventHandler ClipboardHasChanged;
        
        public void OnClipboardChanged(object source, CustomEventArgs args)
        {
            Handle(source, ClipboardHasChanged, args);
        }
    }
}