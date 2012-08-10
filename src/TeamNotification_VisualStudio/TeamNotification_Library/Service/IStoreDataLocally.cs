using System.Collections.Generic;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service
{
    public interface IStoreDataLocally
    {
        void Store(LoginResponse response);
        User GetUser();
        Collection.RedisConfig GetRedisConfig();
    }
}