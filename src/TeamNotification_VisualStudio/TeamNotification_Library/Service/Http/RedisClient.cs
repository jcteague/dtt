using System;

namespace TeamNotification_Library.Service.Http
{
    public class RedisClient : IConnectToRedis
    {
        readonly IRedisConnection conn;

        public RedisClient(IRedisConnection conn)
        {
            this.conn = conn;
        }

        public void Subscribe(string channel,Action<string,byte[]> callback)
        {
            var sub = conn.GetOpenSubscriberChannel();
            sub.Subscribe(channel, callback);
        }
    }
}