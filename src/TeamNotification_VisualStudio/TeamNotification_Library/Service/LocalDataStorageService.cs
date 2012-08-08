using System.Collections.Generic;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Factories;
using TeamNotification_Library.Service.FileSystem;
using System.Linq;

namespace TeamNotification_Library.Service
{
    public class LocalDataStorageService : IStoreDataLocally
    {
        private IHandleFiles resourceHandler;
        private ICreateUserFromResponse userFromResponseGetter;
        private User _user;
        private Collection.RedisConfig _redisConfig;

        public LocalDataStorageService(IHandleFiles resourceHandler, ICreateUserFromResponse userFromResponseGetter)
        {
            this.resourceHandler = resourceHandler;
            this.userFromResponseGetter = userFromResponseGetter;
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

        public void Store(LoginResponse response, IEnumerable<CollectionData> items)
        {
            //_user = userFromResponseGetter.Get(response, items);
            resourceHandler.Write(response);
        }
    }
}