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
    }
}