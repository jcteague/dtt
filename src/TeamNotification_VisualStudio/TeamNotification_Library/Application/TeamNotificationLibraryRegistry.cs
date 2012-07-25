using StructureMap.Configuration.DSL;
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

            For<IRedisConnection>().Use(new Service.Http.RedisConnection(new RedisConnection("10.0.0.37")));
        }
    }
}