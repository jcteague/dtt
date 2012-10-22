using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Async
{
    public interface IHandleToolWindowEvents
    {
        event CustomEventHandler<ToolWindowWasMoved> ToolWindowWasMoved;
        event CustomEventHandler<ToolWindowWasDocked> ToolWindowWasDocked;

        void OnMove(object source, ToolWindowWasMoved eventArgs);
        void OnDockableChange(object source, ToolWindowWasDocked eventArgs);
        void Clear();
    }
}