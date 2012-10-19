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

//        private IInstallableExtension FetchIfUpdate(IInstalledExtension extension, RepositoryEntry entry, IVsExtensionRepository repositoryManager)
//        {
//            var version = extension.Header.Version;
//            try
//            {
//                var newestVersion = repositoryManager.Download(entry);
//                if (newestVersion.Header.Version > extension.Header.Version)
//                {
//                    return newestVersion;
//                }
//            }
//            catch (Exception ex)
//            {
//                // may not have internet connection, etc...
//                Trace.WriteLine(ex.Message);
//            }
//            return null;
//        }

        private RestartReason Install(IVsExtensionManager manager, IInstalledExtension currentExtention, IInstallableExtension updatedExtension)
        {
            // Uninstall old
            manager.Disable(currentExtention);
            manager.Uninstall(currentExtention);

            // Install newer version
            var restartReason = manager.Install(updatedExtension, false);

            // Check newly installed version and enable (which is not the case by default)
            var newlyInstalledVersion = manager.GetInstalledExtension(updatedExtension.Header.Identifier);
            if (newlyInstalledVersion != null)
            {
                manager.Enable(newlyInstalledVersion);
                Trace.WriteLine("Updated: from {0} to {1} at {2}".FormatUsing(currentExtention.Header.Version.ToString(),newlyInstalledVersion.Header.Version.ToString(),newlyInstalledVersion.InstallPath));
            }

            return restartReason;
        }
    }
}