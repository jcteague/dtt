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

        public LocalDataStorageService(IHandleFiles resourceHandler, ICreateUserFromResponse userFromResponseGetter)
        {
            this.resourceHandler = resourceHandler;
            this.userFromResponseGetter = userFromResponseGetter;
        }

        public User GetUser()
        {
            if (_user == null) 
                _user = resourceHandler.Read();
            return _user;
        }
        
        public void Store(User userToStore, IEnumerable<CollectionData> items)
        {
            _user = userFromResponseGetter.Get(userToStore, items);
            resourceHandler.Write(_user);
        }
    }
}