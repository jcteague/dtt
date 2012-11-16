using System;
using Machine.Specifications;
using Microsoft.VisualStudio.ExtensionManager;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Functional;
using TeamNotification_Library.Service.Update;
using developwithpassion.specifications.rhinomocks;
using Rhino.Mocks;

namespace TeamNotification_Test.Library.Service.Update
{
    [Subject(typeof(PackageUpdatesDownloader))]
    public class PackageUpdatesDownloaderSpecs
    {
        public abstract class Concern : Observes<IFetchUpdates, PackageUpdatesDownloader>
        {
            
        }

        public abstract class when_fetching_an_update : Concern
        {
            Establish context = () =>
            {
                extension = fake.an<IInstalledExtension>();
                repositoryManager = fake.an<IVsExtensionRepository>();

                var extensionHeader = fake.an<IExtensionHeader>();
                extension.Stub(x => x.Header).Return(extensionHeader);

                extensionVersion = new Version(1, 0);
                extensionHeader.Stub(x => x.Version).Return(extensionVersion);
            };

            protected static IInstalledExtension extension;
            protected static IVsExtensionRepository repositoryManager;
            protected static Maybe<IInstallableExtension> result;
            protected static Version extensionVersion;
            protected static IInstallableExtension repositoryUpdatedPackage;
        }

        public class when_fetching_an_update_and_there_is_an_updated_version : when_fetching_an_update
        {
            Establish context = () =>
            {
                repositoryUpdatedPackage = fake.an<IInstallableExtension>();
                
                var repositoryPackageHeader = fake.an<IExtensionHeader>();
                repositoryUpdatedPackage.Stub(x => x.Header).Return(repositoryPackageHeader);

                Version versionOnServer = new Version(2, 0);
                repositoryPackageHeader.Stub(x => x.Version).Return(versionOnServer);

                repositoryManager.Stub(x => x.Download(Arg<IRepositoryEntry>.Matches(y => y.DownloadUrl.Contains(GlobalConstants.PackageDownloadUrl)))).Return(repositoryUpdatedPackage);
            };

            Because of = () =>
                result = sut.Fetch(extension, repositoryManager);

            It should_return_the_updated_version = () =>
                result.Value.ShouldEqual(repositoryUpdatedPackage);

        }

        public class when_fetching_the_an_update_and_there_is_no_updated_version : when_fetching_an_update
        {
            Establish context = () =>
            {
                repositoryUpdatedPackage = fake.an<IInstallableExtension>();

                var repositoryPackageHeader = fake.an<IExtensionHeader>();
                repositoryUpdatedPackage.Stub(x => x.Header).Return(repositoryPackageHeader);

                Version versionOnServer = new Version(1, 0);
                repositoryPackageHeader.Stub(x => x.Version).Return(versionOnServer);

                repositoryManager.Stub(x => x.Download(Arg<IRepositoryEntry>.Matches(y => y.DownloadUrl.Contains(GlobalConstants.PackageDownloadUrl)))).Return(repositoryUpdatedPackage);
            };

            Because of = () =>
                result = sut.Fetch(extension, repositoryManager);

            It should_return_nothing = () =>
                result.ShouldBeOfType<Nothing<IInstallableExtension>>();
        }
         
    }
}