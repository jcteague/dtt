using Microsoft.VisualStudio.ExtensionManager;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Service.Async;

namespace TeamNotification_Library.Service.Update
{
    public class DialogForUpdateFactory : ICreateDialogForUpdate
    {
        public DialogMessageWasRequested GetInstance(IUpdatePackage packageUpdateManager, IInstalledExtension updatedExtension, IVsExtensionManager extensionManager, IVsExtensionRepository repoManager)
        {
            return new DialogMessageWasRequested
                                           {
                                               Message = "There is an update available for {0}. Would you like to download it?".FormatUsing(GlobalConstants.PackageName),
                                               OkAction = () => packageUpdateManager.UpdateIfAvailable(updatedExtension, extensionManager, repoManager),
                                               CancelAction = () => { }
                                           };
        }
    }
}