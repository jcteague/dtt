using Microsoft.VisualStudio.ExtensionManager;

namespace TeamNotification_Library.Service.Update
{
    public interface IInstallUpdates
    {
        RestartReason Install(IVsExtensionManager manager, IInstalledExtension currentExtention, IInstallableExtension updatedExtension);
    }
}