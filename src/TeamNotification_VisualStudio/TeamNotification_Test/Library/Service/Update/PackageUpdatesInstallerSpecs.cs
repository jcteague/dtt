using Machine.Specifications;
using Microsoft.VisualStudio.ExtensionManager;
using TeamNotification_Library.Service.Update;
using developwithpassion.specifications.rhinomocks;
using Rhino.Mocks;

namespace TeamNotification_Test.Library.Service.Update
{
    [Subject(typeof(PackageUpdatesInstaller))]
    public class PackageUpdatesInstallerSpecs
    {
        public abstract class Concern : Observes<IInstallUpdates>
        {
            
        }

        public abstract class when_installing_an_update : Concern
        {
            Establish context = () =>
            {
                extensionManager = fake.an<IVsExtensionManager>();
                currentExtension = fake.an<IInstalledExtension>();
                updatedExtension = fake.an<IInstallableExtension>();

                var updatedExtensionHeader = fake.an<IExtensionHeader>();
                updatedExtension.Stub(x => x.Header).Return(updatedExtensionHeader);

                updatedVersionIdentifier = "blah identifier";
                updatedExtensionHeader.Stub(x => x.Identifier).Return(updatedVersionIdentifier);
            };

            protected static IVsExtensionManager extensionManager;
            protected static IInstalledExtension currentExtension;
            protected static IInstallableExtension updatedExtension;
            protected static string updatedVersionIdentifier;
        }

        public class when_installing_an_update_and_the_newly_installed_version_is_defined : when_installing_an_update
        {
            Establish context = () =>
            {
                updatedExtensionInstalled = fake.an<IInstalledExtension>();
                extensionManager.Stub(x => x.GetInstalledExtension(updatedVersionIdentifier)).Return(updatedExtensionInstalled);
            };

            Because of = () =>
                sut.Install(extensionManager, currentExtension, updatedExtension);

            It should_disable_the_current_extension = () =>
                extensionManager.AssertWasCalled(x => x.Disable(currentExtension));

            It should_uninstall_the_current_extension = () =>
                extensionManager.AssertWasCalled(x => x.Uninstall(currentExtension));

            It should_install_the_new_extension = () =>
                extensionManager.AssertWasCalled(x => x.Install(updatedExtension, false));

            It should_enable_the_newly_installed_extension = () =>
                extensionManager.AssertWasCalled(x => x.Enable(updatedExtensionInstalled));

            private static IInstalledExtension updatedExtensionInstalled;
        }
         
    }
}