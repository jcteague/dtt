﻿using StructureMap;
using TeamNotification_Library.Application;

namespace AvenidaSoftware.TeamNotification
{
    public class Bootstrapper
    {
        public static void Initialize()
        {
            InitializeStructureMap();
        }

        static void InitializeStructureMap()
        {
            ObjectFactory.Initialize(cfg=>
            {
                cfg.AddRegistry<TeamNotificationLibraryRegistry>();
                cfg.Scan(scanner=>
                {
                    scanner.AssemblyContainingType<MyControl>();
                    scanner.RegisterConcreteTypesAgainstTheFirstInterface().OnAddedPluginTypes(x=>x.Singleton());
                });
            });
        }
    }
}