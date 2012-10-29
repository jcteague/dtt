using Microsoft.VisualStudio.ExtensionManager;

namespace TeamNotification_Library.Service.Update
{
    public interface ICheckForUpdates
    {
        void CheckForUpdates(IVsExtensionManager updateManager, IVsExtensionRepository repoManager);
    }
}