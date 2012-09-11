using TeamNotification_Library.Configuration;

namespace TeamNotification_Library.Service.ToolWindow
{
    public class ToolWindowActionGetter : IGetToolWindowAction
    {
        private IGetToolWindowPosition toolWindowPositionGetter;

        public ToolWindowActionGetter(IGetToolWindowPosition toolWindowPositionGetter)
        {
            this.toolWindowPositionGetter = toolWindowPositionGetter;
        }

        public IActOnChatElements Get()
        {
            var position = toolWindowPositionGetter.Get();

            switch (position)
            {
                case GlobalConstants.DockPositions.Left:
                case GlobalConstants.DockPositions.Right:
                    return new MessageInputAtBottom();

                default:
                    return new MessageInputAtRight();
            }
        }
    }
}