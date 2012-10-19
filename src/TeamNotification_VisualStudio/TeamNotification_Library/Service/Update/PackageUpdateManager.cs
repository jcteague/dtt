using System;
using System.Diagnostics;
using System.IO.Packaging;
using System.Linq;
using Microsoft.VisualStudio.ExtensionManager;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Extensions;

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

            var version = packageUpdatesDownloader.Fetch(teamNotificationExtension, repositoryManager);
            if (version.IsDefined)
            {
                packageInstaller.Install(extensionManager, teamNotificationExtension, version.Value);
            }

            return version.IsDefined;
        }
    }
}