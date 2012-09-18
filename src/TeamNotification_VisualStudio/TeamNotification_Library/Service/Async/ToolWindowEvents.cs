using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Async
{
    public class ToolWindowEvents : AbstractEventHandler, IHandleToolWindowEvents
    {
        public event CustomEventHandler<ToolWindowWasMoved> ToolWindowWasMoved;
        public event CustomEventHandler<ToolWindowWasDocked> ToolWindowWasDocked;

        public void OnMove(object source, ToolWindowWasMoved eventArgs)
        {
            Handle(source, ToolWindowWasMoved, eventArgs);
        }

        public void OnDockableChange(object source, ToolWindowWasDocked eventArgs)
        {
            Handle(source, ToolWindowWasDocked, eventArgs);
        }

        public void ClearAll()
        {
            ToolWindowWasMoved = null;
            ToolWindowWasDocked = null;
        }
    }
}