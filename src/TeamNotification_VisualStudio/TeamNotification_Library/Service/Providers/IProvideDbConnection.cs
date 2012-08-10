using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Service.Http;

namespace TeamNotification_Library.Service.Providers
{
    public interface IProvideDbConnection<T> where T : IRedisConnection
    {
        T Get();

        void Set(string host, int port);
    }

    public class DbConnectionProvider : IProvideDbConnection<IRedisConnection>
    {
        private IRedisConnection redisConnection;
        private IProvideConfiguration<RedisConfiguration> redisConfiguration;

        public DbConnectionProvider(IProvideConfiguration<RedisConfiguration> redisConfigurationProvider)
        {
            redisConnection = null;
            redisConfiguration = redisConfigurationProvider;
        }
        
        public IRedisConnection Get()
        {
            if(redisConnection == null)
            {
                string[] redisConfigArray = redisConfiguration.Get().Uri.Split(':');

                Set(redisConfigArray[0], Convert.ToInt32(redisConfigArray[1]));
            }
            return redisConnection;
        }

        public void Set(string host, int port)
        {
            redisConnection = new RedisConnection(new BookSleeve.RedisConnection(host, port));
        }
    }
}