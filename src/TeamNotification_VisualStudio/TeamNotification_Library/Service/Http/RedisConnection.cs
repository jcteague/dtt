using BookSleeve;

namespace TeamNotification_Library.Service.Http
{
    public class RedisConnection : IRedisConnection
    {
        readonly BookSleeve.RedisConnection conn;

        public RedisConnection(BookSleeve.RedisConnection conn)
        {
            this.conn = conn;
        }

        public void Open()
        {
            conn.Open();
        }

        public void Close()
        {
            conn.Dispose();
        }

        public RedisSubscriberConnection GetOpenSubscriberChannel()
        {
            return conn.GetOpenSubscriberChannel();
        }
    }
}