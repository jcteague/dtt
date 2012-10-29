using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Machine.Specifications;
using Microsoft.VisualStudio.ExtensionManager;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.Mappers;
using TeamNotification_Library.Service.Update;
using developwithpassion.specifications.rhinomocks;
using Rhino.Mocks;

namespace TeamNotification_Test.Library.Service.Update
{
    [Subject(typeof(PackageUpdateVerifier))]
    public class PackageUpdateVerifierSpecs
    {
        public abstract class Concern : Observes<ICheckForUpdates, PackageUpdateVerifier>
        {
            Establish context = () =>
            {
                configuration = depends.on<IProvideConfiguration<PluginServerConfiguration>>();
                packageUpdateManager = depends.on<IUpdatePackage>();
                httpClient = depends.on<ISendHttpRequests>();
                collectionPluginToPluginMapper = depends.on<IMapEntities<Collection.Plugin, Plugin>>();
                dialogMessagesEvents = depends.on<IHandleDialogMessages>();
                dialogMessageEventFactory = depends.on<ICreateDialogForUpdate>();

            };
            
            protected static IProvideConfiguration<PluginServerConfiguration> configuration;
            protected static IUpdatePackage packageUpdateManager;
            protected static ISendHttpRequests httpClient;
            protected static IMapEntities<Collection.Plugin, Plugin> collectionPluginToPluginMapper;
            protected static IHandleDialogMessages dialogMessagesEvents;
            protected static ICreateDialogForUpdate dialogMessageEventFactory;
        }

        public abstract class when_checking_for_updates : Concern
        {
            Establish context = () =>
            {
                IStoreConfiguration pluginServerConfiguration = new PluginServerConfiguration {Uri = "blah uri"};
                configuration.Stub(x => x.Get()).Return(pluginServerConfiguration);


                CollectionData collectionData1 = new CollectionData {name = "version"};
                IEnumerable<CollectionData> collectionData = new List<CollectionData> {collectionData1};
                pluginCollection = new Collection {plugin = new Collection.Plugin {data = collectionData}};
                Task<Collection> collectionResponseTask = Task<Collection>.Factory.StartNew(() => pluginCollection);
                httpClient.Stub(x => x.Get<Collection>(pluginServerConfiguration.Uri)).Return(collectionResponseTask);

                extensionManager = fake.an<IVsExtensionManager>();
                repositoryManager = fake.an<IVsExtensionRepository>();

                Func<string, IInstalledExtension> prepareFakeExtension = (headerName) =>
                {
                    var extension =
                        fake.an<IInstalledExtension>();
                    IExtensionHeader header = fake.an<IExtensionHeader>();
                    extension.Stub(x => x.Header).Return(header);

                    header.Stub(x => x.Name).Return(headerName);

                    var headerVersion = new Version("1.0");
                    header.Stub(x => x.Version).Return(headerVersion);

                    return extension;
                };

                notOurExtension = prepareFakeExtension("blah header");
                ourInstalledPackage = prepareFakeExtension(GlobalConstants.PackageName);

                IEnumerable<IInstalledExtension> installedExtensions = new List<IInstalledExtension> { notOurExtension, ourInstalledPackage };
                extensionManager.Stub(x => x.GetInstalledExtensions()).Return(installedExtensions);
            };

            protected static Collection pluginCollection;
            protected static IVsExtensionManager extensionManager;
            protected static IVsExtensionRepository repositoryManager;
            protected static IInstalledExtension ourInstalledPackage;
            protected static IInstalledExtension notOurExtension;
        }

        public class when_checking_for_updates_and_the_current_version_is_the_same_as_the_version_in_the_server : when_checking_for_updates
        {
            Establish context = () =>
            {
                Plugin plugin = new Plugin {Version = new Version("1.0")};
                collectionPluginToPluginMapper.Stub(x => x.MapFrom(pluginCollection.plugin)).Return(plugin);
            };

            Because of = () =>
                sut.CheckForUpdates(extensionManager, repositoryManager);

            It should_not_show_any_dialog = () =>
                dialogMessagesEvents.AssertWasNotCalled(x => x.OnDialogMessageRequested(Arg<object>.Is.Anything, Arg<DialogMessageWasRequested>.Is.Anything));
        }

        public class when_checking_for_updates_and_the_current_version_is_below_the_version_in_the_server : when_checking_for_updates
        {
            Establish context = () =>
            {
                Plugin plugin = new Plugin {Version = new Version("1.1")};
                collectionPluginToPluginMapper.Stub(x => x.MapFrom(pluginCollection.plugin)).Return(plugin);

                dialogMessageWasRequested = fake.an<DialogMessageWasRequested>();
                dialogMessageEventFactory.Stub(x => x.GetInstance(packageUpdateManager, ourInstalledPackage, extensionManager, repositoryManager)).Return(dialogMessageWasRequested);
            };

            Because of = () =>
                sut.CheckForUpdates(extensionManager, repositoryManager);

            It should_show_a_dialog = () =>
                dialogMessagesEvents.AssertWasCalled(x => x.OnDialogMessageRequested(sut, dialogMessageWasRequested));

            private static DialogMessageWasRequested dialogMessageWasRequested;
        }
         
    }
}