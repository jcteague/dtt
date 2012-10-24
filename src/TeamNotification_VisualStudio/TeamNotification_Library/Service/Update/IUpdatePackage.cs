using Microsoft.VisualStudio.ExtensionManager;

namespace TeamNotification_Library.Service.Update
{
    public interface IUpdatePackage
    {
        bool UpdateIfAvailable(IVsExtensionManager extensionManager, IVsExtensionRepository repositoryManager);
        bool UpdateIfAvailable(IInstalledExtension updatedExtension, IVsExtensionManager extensionManager, IVsExtensionRepository repositoryManager);
    }
}