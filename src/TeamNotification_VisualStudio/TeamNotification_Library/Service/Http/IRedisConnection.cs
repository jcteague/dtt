using BookSleeve;

namespace TeamNotification_Library.Service.Http
{
    public interface IRedisConnection
    {
        void Open();
        void Close();
        RedisSubscriberConnection GetOpenSubscriberChannel();
    }
}