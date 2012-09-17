using System.Collections.Generic;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.FileSystem;
using System.Linq;

namespace TeamNotification_Library.Service
{
    public class LocalDataStorageService : IStoreDataLocally
    {
        private IHandleFiles resourceHandler;
        private User _user;
        private Collection.RedisConfig _redisConfig;

        public LocalDataStorageService(IHandleFiles resourceHandler)
        {
            this.resourceHandler = resourceHandler;
        }

        public User GetUser()
        {
            if (_user == null )
            {
                var result = resourceHandler.Read();
                if(result != null)
                    _user = result.user;
            }
            return _user;
        }

        public Collection.RedisConfig GetRedisConfig()
        {
            if (_redisConfig == null)
            {
                var result = resourceHandler.Read();
                if (result != null)
                    _redisConfig = result.redis;
            }
            return _redisConfig;
        }

        public void DeleteUser()
        {
            resourceHandler.Delete();
        }

        public void Store(LoginResponse response)
        {
            resourceHandler.Write(response);
        }
    }
}