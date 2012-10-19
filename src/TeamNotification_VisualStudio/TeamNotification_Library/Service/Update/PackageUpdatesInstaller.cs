using Microsoft.VisualStudio.ExtensionManager;
using TeamNotification_Library.Extensions;

namespace TeamNotification_Library.Service.Update
{
    public class PackageUpdatesInstaller : IInstallUpdates
    {
        public void Install(IVsExtensionManager manager, IInstalledExtension currentExtention, IInstallableExtension updatedExtension)
        {
            manager.Disable(currentExtention);
            manager.Uninstall(currentExtention);
            manager.Install(updatedExtension, false);

            var newlyInstalledVersion = manager.GetInstalledExtension(updatedExtension.Header.Identifier);
            if (newlyInstalledVersion.IsNotNull())
            {
                manager.Enable(newlyInstalledVersion);
            }
        }

//        private RestartReason Install(IVsExtensionManager manager, IInstalledExtension currentExtention, IInstallableExtension updatedExtension)
//        {
//            // Uninstall old
//            manager.Disable(currentExtention);
//            manager.Uninstall(currentExtention);
//
//            // Install newer version
//            var restartReason = manager.Install(updatedExtension, false);
//
//            // Check newly installed version and enable (which is not the case by default)
//            var newlyInstalledVersion = manager.GetInstalledExtension(updatedExtension.Header.Identifier);
//            if (newlyInstalledVersion != null)
//            {
//                manager.Enable(newlyInstalledVersion);
//                Trace.WriteLine("Updated: from {0} to {1} at {2}".FormatUsing(currentExtention.Header.Version.ToString(), newlyInstalledVersion.Header.Version.ToString(), newlyInstalledVersion.InstallPath));
//            }
//
//            return restartReason;
//        }
    }
}