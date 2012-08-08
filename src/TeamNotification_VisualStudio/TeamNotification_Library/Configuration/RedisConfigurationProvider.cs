using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeamNotification_Library.Service;

namespace TeamNotification_Library.Configuration
{
    public class RedisConfigurationProvider : IProvideConfiguration<RedisConfiguration>
    {
        private static RedisConfiguration redisConfiguration;

        public IStoreConfiguration Get()
        {
            if (redisConfiguration == null)
                redisConfiguration = new RedisConfiguration();
            return redisConfiguration;
        }
    }
}
