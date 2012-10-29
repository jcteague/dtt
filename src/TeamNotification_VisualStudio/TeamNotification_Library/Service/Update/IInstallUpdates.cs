using Microsoft.VisualStudio.ExtensionManager;

namespace TeamNotification_Library.Service.Update
{
    public interface IInstallUpdates
    {
        void Install(IVsExtensionManager manager, IInstalledExtension currentExtention, IInstallableExtension updatedExtension);
    }
}