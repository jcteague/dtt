using StructureMap.Configuration.DSL;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Service;
using TeamNotification_Library.Service.Clipboard;
using TeamNotification_Library.Service.Factories.UI;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.LocalSystem;

namespace TeamNotification_Library.Application
{
    public class TeamNotificationLibraryRegistry : Registry
    {
        public TeamNotificationLibraryRegistry()
        {
            Scan(scanner =>
            {
                scanner.TheCallingAssembly();
                scanner.RegisterConcreteTypesAgainstTheFirstInterface().OnAddedPluginTypes(x => x.Singleton());
            });

            For<IProvideConfiguration<RedisConfiguration>>().Singleton().Use<RedisConfigurationProvider>();
            For<IStoreGlobalState>().Singleton();
            For<IStoreDataLocally>().Singleton();
            For<IStoreClipboardData>().Singleton();
            For<IHandleSystemClipboard>().Singleton();
            For<ICreateSyntaxBlockUIInstances>().Singleton();
        }
    }
}