using TeamNotification_Library.Configuration;

namespace TeamNotification_Library.Service.ToolWindow
{
    public class ToolWindowActionGetter : IGetToolWindowAction
    {
        private IGetToolWindowOrientation toolWindowOrientationGetter;

        public ToolWindowActionGetter(IGetToolWindowOrientation toolWindowOrientationGetter)
        {
            this.toolWindowOrientationGetter = toolWindowOrientationGetter;
        }

        public IActOnChatElements Get()
        {
            var orientation = toolWindowOrientationGetter.Get();

            if (orientation == GlobalConstants.DockOrientations.InputAtBottom)
                return new MessageInputAtBottom();

            return new MessageInputAtRight();
        }
    }
}