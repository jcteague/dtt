using System;
using TeamNotification_Library.Service.Providers;

namespace TeamNotification_Library.Service.Http
{
    public class RedisClient : IConnectToRedis
    {
        readonly IRedisConnection conn;

        public RedisClient(IProvideDbConnection<IRedisConnection> connectionProvider )// IRedisConnection conn)
        {
            this.conn = connectionProvider.Get();
        }

        public void Subscribe(string channel,Action<string,byte[]> callback)
        {
            var sub = conn.GetOpenSubscriberChannel();
            sub.Subscribe(channel, callback);
        }
    }
}