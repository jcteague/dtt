using System;
using System.Diagnostics;
using Microsoft.VisualStudio.ExtensionManager;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Functional;
using TeamNotification_Library.Extensions;

namespace TeamNotification_Library.Service.Update
{
    public class PackageUpdatesDownloader : IFetchUpdates
    {
        public Maybe<IInstallableExtension> Fetch(IInstalledExtension extension, IVsExtensionRepository repositoryManager)
        {
            var entry = new RepositoryEntry
                            {
                                Name = GlobalConstants.PackageName,
                                DownloadUrl = Properties.Settings.Default.Site + GlobalConstants.PackageDownloadUrl,
                                VsixReferences = GlobalConstants.PackageName
                            };
            
            var newestVersion = repositoryManager.Download(entry);
            if (newestVersion.Header.Version > extension.Header.Version)
            {
                return newestVersion.ToMaybe();
            }

            return new Nothing<IInstallableExtension>();
        }
    }
}