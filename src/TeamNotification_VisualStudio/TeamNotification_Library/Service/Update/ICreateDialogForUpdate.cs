using Microsoft.VisualStudio.ExtensionManager;
using TeamNotification_Library.Service.Async;

namespace TeamNotification_Library.Service.Update
{
    public interface ICreateDialogForUpdate
    {
        DialogMessageWasRequested GetInstance(IUpdatePackage packageUpdateManager, IInstalledExtension updatedExtension, IVsExtensionManager extensionManager, IVsExtensionRepository repoManager);
    }
}