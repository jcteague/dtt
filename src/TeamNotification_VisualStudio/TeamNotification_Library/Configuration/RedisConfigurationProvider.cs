using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamNotification_Library.Configuration
{
    public class RedisConfigurationProvider : IProvideConfiguration<RedisConfiguration>
    {
        public IStoreConfiguration Get()
        {
            return new RedisConfiguration();
        }
    }
}
