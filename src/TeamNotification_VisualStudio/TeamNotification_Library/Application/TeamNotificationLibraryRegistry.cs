﻿using StructureMap.Configuration.DSL;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Service;
using TeamNotification_Library.Service.Clipboard;
using TeamNotification_Library.Service.Http;

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
            For<IProvideConfiguration<RedisConfiguration>>().Singleton().Use<RedisConfigurationProvider>();
            For<IStoreDataLocally>().Singleton().Use<LocalDataStorageService>();
            For<IStoreClipboardData>().Singleton().Use<ClipboardDataStorageService>();
        }
    }
}