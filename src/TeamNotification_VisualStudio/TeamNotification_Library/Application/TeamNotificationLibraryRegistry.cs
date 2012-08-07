using StructureMap.Configuration.DSL;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Service;
using TeamNotification_Library.Service.Http;
using RedisConnection = BookSleeve.RedisConnection;

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

           // For<IRedisConnection>().Use(new Service.Http.RedisConnection(new RedisConnection("dtt.local")));
            For<IStoreDataLocally>().Singleton().Use<LocalDataStorageService>();
        }
    }
}