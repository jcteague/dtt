using TeamNotification_Library.Configuration;
using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.ToolWindow
{
    public class ToolWindowActionGetter : IGetToolWindowAction
    {
        private IGetToolWindowPosition toolWindowPositionGetter;

        public ToolWindowActionGetter(IGetToolWindowPosition toolWindowPositionGetter)
        {
            this.toolWindowPositionGetter = toolWindowPositionGetter;
        }

        public IActOnChatElements Get(ToolWindowWasDocked toolWindowWasDockedArgs)
        {
            var position = toolWindowPositionGetter.Get(toolWindowWasDockedArgs.x, toolWindowWasDockedArgs.y,
                                                        toolWindowWasDockedArgs.w, toolWindowWasDockedArgs.h,
                                                        toolWindowWasDockedArgs.isDocked);

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