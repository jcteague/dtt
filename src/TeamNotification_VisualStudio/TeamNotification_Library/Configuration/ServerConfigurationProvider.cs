using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamNotification_Library.Configuration
{
    public class ServerConfigurationProvider : IProvideConfiguration<ServerConfiguration>
    {
        public IStoreConfiguration Get()
        {
            return new ServerConfiguration();
        }
    }
}
