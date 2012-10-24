using System;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Mappers
{
    public class CollectionPluginToPluginMapper : IMapEntities<Collection.Plugin, Plugin>
    {
        public Plugin MapFrom(Collection.Plugin source)
        {
            var name = Collection.getField(source.data, "name");
            var version = Collection.getField(source.data, "version");
            return new Plugin
                       {
                           Name = name,
                           Version = new Version(version)
                       };
        }
    }
}