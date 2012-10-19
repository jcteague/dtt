using Microsoft.VisualStudio.ExtensionManager;
using TeamNotification_Library.Functional;

namespace TeamNotification_Library.Service.Update
{
    public interface IFetchUpdates
    {
        Maybe<IInstallableExtension> Fetch(IInstalledExtension extension, IVsExtensionRepository repositoryManager);
    }
}