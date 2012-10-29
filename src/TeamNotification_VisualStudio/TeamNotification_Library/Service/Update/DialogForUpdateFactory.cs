using System.ComponentModel;
using Microsoft.VisualStudio.ExtensionManager;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Service.Async;

namespace TeamNotification_Library.Service.Update
{
    public class DialogForUpdateFactory : ICreateDialogForUpdate
    {
        private IRunInBackgroundWorker backgroundRunner;
        private IHandleDialogMessages dialogMessagesEvents;

        public DialogForUpdateFactory(IRunInBackgroundWorker backgroundRunner, IHandleDialogMessages dialogMessagesEvents)
        {
            this.backgroundRunner = backgroundRunner;
            this.dialogMessagesEvents = dialogMessagesEvents;
        }

        public DialogMessageWasRequested GetInstance(IUpdatePackage packageUpdateManager, IInstalledExtension updatedExtension, IVsExtensionManager extensionManager, IVsExtensionRepository repoManager)
        {
            return new DialogMessageWasRequested
                                           {
                                               Message = "There is an update available for {0}. Would you like to download it?".FormatUsing(GlobalConstants.PackageName),
                                               OkAction = () => backgroundRunner.Run(() =>
                                                                                         {
                                                                                             var updated = packageUpdateManager.UpdateIfAvailable(updatedExtension, extensionManager, repoManager);
                                                                                             if (updated)
                                                                                             {
                                                                                                dialogMessagesEvents.OnAlertMessageRequested(this, new AlertMessageWasRequested {Message = "You must restart for the update to take effect."});
                                                                                             }
                                                                                         }),
                                               CancelAction = () => { }
                                           };
        }
    }
}