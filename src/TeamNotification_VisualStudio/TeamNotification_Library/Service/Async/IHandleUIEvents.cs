using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Async
{
    public interface IHandleUIEvents
    {
        event CustomEventHandler<ControlRenderWasRequested> ControlRenderWasRequested;
        void OnControlRenderWasRequested(object source, ControlRenderWasRequested eventArgs);
        void Clear();
    }
}