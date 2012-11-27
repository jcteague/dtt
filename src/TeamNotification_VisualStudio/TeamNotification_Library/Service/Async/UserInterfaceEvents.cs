using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Async
{
    public class UserInterfaceEvents : AbstractEventHandler, IHandleUIEvents
    {
        public event CustomEventHandler<ControlRenderWasRequested> ControlRenderWasRequested;

        public void OnControlRenderWasRequested(object source, ControlRenderWasRequested eventArgs)
        {
            Handle(source, ControlRenderWasRequested, eventArgs);
        }

        public void Clear()
        {
            ControlRenderWasRequested = null;
        }
    }
}