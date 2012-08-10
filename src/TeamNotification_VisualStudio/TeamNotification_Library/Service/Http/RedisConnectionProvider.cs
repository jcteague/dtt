using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeamNotification_Library.Service.Providers;

namespace TeamNotification_Library.Service.Http
{
    public class RedisConnectionProvider : IProvideDbConnection<IRedisConnection>
    {
        public IRedisConnection Get()
        {
            return new IRedisConnection();
        }
    }
}
