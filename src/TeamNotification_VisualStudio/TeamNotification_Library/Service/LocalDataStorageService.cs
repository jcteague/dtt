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

        public LocalDataStorageService(IHandleFiles resourceHandler)
        {
            this.resourceHandler = resourceHandler;
        }

        public User GetUser()
        {
            if (_user == null) 
                _user = resourceHandler.Read();
            return _user;
        }
        
        public void Store(User userToStore, IEnumerable<CollectionData> items)
        {
            var data = GetResourceFrom(userToStore, items);
            resourceHandler.Write(data);
            _user = new User
                        {
                            id = userToStore.id,
                            email = userToStore.email,
                            password = data.First(x => x.name == Globals.Fields.Password).value
                        };
        }

        private IList<CollectionData> GetResourceFrom(User userToStore, IEnumerable<CollectionData> items)
        {
            var data = new List<CollectionData>();
            data.Add(new CollectionData {name = "id", value = userToStore.id.ToString()});
            data.AddRange(items);
            return data;
        }
    }
}