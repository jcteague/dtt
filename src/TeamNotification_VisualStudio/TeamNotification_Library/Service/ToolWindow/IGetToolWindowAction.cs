using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.ToolWindow
{
    public interface IGetToolWindowAction
    {
        IActOnChatElements Get(ToolWindowWasDocked toolWindowWasDockedArgs);
    }
}