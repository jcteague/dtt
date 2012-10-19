using System;
using System.Collections.Generic;
using Machine.Specifications;
using Microsoft.VisualStudio.ExtensionManager;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Functional;
using TeamNotification_Library.Service.Update;
using developwithpassion.specifications.rhinomocks;
using Rhino.Mocks;

namespace TeamNotification_Test.Library.Service.Update
{
    [Subject(typeof(PackageUpdateManager))]  
    public class PackageUpdateManagerSpecs
    {
        public abstract class Concern : Observes<IUpdatePackage, PackageUpdateManager>
        {
            Establish context = () =>
            {
                packageUpdatesDownloader = depends.on<IFetchUpdates>();
                packageInstaller = depends.on<IInstallUpdates>();
            };
            
            protected static IFetchUpdates packageUpdatesDownloader;
            protected static IInstallUpdates packageInstaller;
        }

        public abstract class when_updating_the_package : Concern
        {
            Establish context = () =>
            {
                extensionManager = fake.an<IVsExtensionManager>();
                repositoryManager = fake.an<IVsExtensionRepository>();

                Func<string, IInstalledExtension> prepareFakeExtension = (headerName) =>
                {
                    var extension =
                        fake.an<IInstalledExtension>();
                    IExtensionHeader header = fake.an<IExtensionHeader>();
                    extension.Stub(x => x.Header).Return(header);

                    header.Stub(x => x.Name).Return(headerName);

                    return extension;
                };

                notOurExtension = prepareFakeExtension("blah header");
                ourInstalledPackage = prepareFakeExtension(GlobalConstants.PackageName);

                IEnumerable<IInstalledExtension> installedExtensions = new List<IInstalledExtension> { notOurExtension, ourInstalledPackage };
                extensionManager.Stub(x => x.GetInstalledExtensions()).Return(installedExtensions);
            };

            protected static IVsExtensionManager extensionManager;
            protected static IVsExtensionRepository repositoryManager;
            protected static IInstalledExtension ourInstalledPackage;
            protected static IInstalledExtension notOurExtension;

            protected static bool result;
        }

        public class when_updating_the_package_and_there_is_an_updated_version_available : when_updating_the_package
        {
            Establish context = () =>
            {
                newVersionValue = fake.an<IInstallableExtension>();
                Maybe<IInstallableExtension> newVersion = new Just<IInstallableExtension>(newVersionValue);
                packageUpdatesDownloader.Stub(x => x.Fetch(ourInstalledPackage, repositoryManager)).Return(newVersion);
            };

            Because of = () =>
                result = sut.UpdateIfAvailable(extensionManager, repositoryManager);

            It should_install_the_package = () =>
                packageInstaller.AssertWasCalled(x => x.Install(extensionManager, ourInstalledPackage, newVersionValue));

            It should_return_true = () =>
                result.ShouldBeTrue();


            private static IInstallableExtension newVersionValue;
        }

        public class when_updating_the_package_and_there_is_not_an_updated_version_available : when_updating_the_package
        {
            Establish context = () =>
            {
                Maybe<IInstallableExtension> newVersion = new Nothing<IInstallableExtension>();
                packageUpdatesDownloader.Stub(x => x.Fetch(ourInstalledPackage, repositoryManager)).Return(newVersion);
            };

            Because of = () =>
                result = sut.UpdateIfAvailable(extensionManager, repositoryManager);

            It should_not_install_the_package = () =>
                packageInstaller.AssertWasNotCalled(x => 
                    x.Install(Arg<IVsExtensionManager>.Is.Anything, Arg<IInstalledExtension>.Is.Anything, Arg<IInstallableExtension>.Is.Anything));

            It should_return_false = () =>
                result.ShouldBeFalse();
        }
    }
}