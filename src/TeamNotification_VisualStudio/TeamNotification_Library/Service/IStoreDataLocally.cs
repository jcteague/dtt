using TeamNotification_Library.Configuration;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service
{
    public interface IStoreDataLocally
    {
        void Store(User user);
        RedisConfiguration
        User User { get; set; }
    }
}