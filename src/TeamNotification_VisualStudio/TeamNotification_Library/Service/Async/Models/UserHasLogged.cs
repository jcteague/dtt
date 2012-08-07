using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Async.Models
{
    public class UserHasLogged : IHaveEventArguments
    {
        public User User { get; set; }
        public Collection.RedisConfig RedisConfig { get; set; }

        public UserHasLogged(User user, Collection.RedisConfig redis)
        {
            User = user;
            RedisConfig = redis;
        }
    }
}