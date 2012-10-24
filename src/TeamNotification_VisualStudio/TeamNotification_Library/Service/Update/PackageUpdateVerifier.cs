using System.Linq;
using Microsoft.VisualStudio.ExtensionManager;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Service.Mappers;

namespace TeamNotification_Library.Service.Update
{
    public class PackageUpdateVerifier : ICheckForUpdates
    {
        private IProvideConfiguration<PluginServerConfiguration> configuration;
        private IUpdatePackage packageUpdateManager;
        private ISendHttpRequests httpClient;
        private IMapEntities<Collection.Plugin, Plugin> collectionPluginToPluginMapper;
        private IHandleDialogMessages dialogMessagesEvents;
        private ICreateDialogForUpdate dialogMessageEventFactory;

        public PackageUpdateVerifier(IProvideConfiguration<PluginServerConfiguration> configuration, IUpdatePackage packageUpdateManager, ISendHttpRequests httpClient, IMapEntities<Collection.Plugin, Plugin> collectionPluginToPluginMapper, IHandleDialogMessages dialogMessagesEvents, ICreateDialogForUpdate dialogMessageEventFactory)
        {
            this.configuration = configuration;
            this.packageUpdateManager = packageUpdateManager;
            this.httpClient = httpClient;
            this.collectionPluginToPluginMapper = collectionPluginToPluginMapper;
            this.dialogMessagesEvents = dialogMessagesEvents;
            this.dialogMessageEventFactory = dialogMessageEventFactory;
        }

        public void CheckForUpdates(IVsExtensionManager extensionManager, IVsExtensionRepository repoManager)
        {
            var plugin = collectionPluginToPluginMapper.MapFrom(GetCollection().plugin);
            var updatedExtension = extensionManager.GetInstalledExtensions().FirstOrDefault(x => GlobalConstants.PackageName == x.Header.Name && x.Header.Version < plugin.Version);

            if (updatedExtension.IsNotNull())
            {
                var dialogParameters = dialogMessageEventFactory.GetInstance(packageUpdateManager, updatedExtension, extensionManager, repoManager);
                dialogMessagesEvents.OnDialogMessageRequested(this, dialogParameters);
            }
        }

        private Collection GetCollection()
        {
            var uri = configuration.Get().Uri;
            return httpClient.Get<Collection>(uri).Result;
        }
    }
}