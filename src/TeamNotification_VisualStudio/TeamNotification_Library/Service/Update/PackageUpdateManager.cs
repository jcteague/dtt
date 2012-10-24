using System.Linq;
using Microsoft.VisualStudio.ExtensionManager;
using TeamNotification_Library.Configuration;

namespace TeamNotification_Library.Service.Update
{
    public class PackageUpdateManager : IUpdatePackage
    {
        private IFetchUpdates packageUpdatesDownloader;
        private IInstallUpdates packageInstaller;

        public PackageUpdateManager(IFetchUpdates packageUpdatesDownloader, IInstallUpdates packageInstaller)
        {
            this.packageUpdatesDownloader = packageUpdatesDownloader;
            this.packageInstaller = packageInstaller;
        }

        public bool UpdateIfAvailable(IVsExtensionManager extensionManager, IVsExtensionRepository repositoryManager)
        {
            var teamNotificationExtension =
                extensionManager.GetInstalledExtensions().First(x => GlobalConstants.PackageName == x.Header.Name);

            return UpdateIfAvailable(teamNotificationExtension, extensionManager, repositoryManager);
        }

        public bool UpdateIfAvailable(IInstalledExtension teamNotificationExtension, IVsExtensionManager extensionManager, IVsExtensionRepository repositoryManager)
        {
            var version = packageUpdatesDownloader.Fetch(teamNotificationExtension, repositoryManager);
            if (version.IsDefined)
            {
                packageInstaller.Install(extensionManager, teamNotificationExtension, version.Value);
            }

            return version.IsDefined;
        }
    }
}