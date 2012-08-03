using System.Collections.Generic;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.FileSystem;

namespace TeamNotification_Library.Service
{
    public class LocalDataStorageService : IStoreDataLocally
    {
        private IHandleFiles resourceHandler;

        public LocalDataStorageService(IHandleFiles resourceHandler)
        {
            this.resourceHandler = resourceHandler;
        }

        public User GetUser()
        {
            return resourceHandler.Read();
        }
        
        public void Store(User userToStore, IEnumerable<CollectionData> items)
        {
            var data = GetResourceFrom(userToStore, items);
            resourceHandler.Write(data);
        }

        private IEnumerable<CollectionData> GetResourceFrom(User userToStore, IEnumerable<CollectionData> items)
        {
            var data = new List<CollectionData>();
            data.Add(new CollectionData {name = "id", value = userToStore.id.ToString()});
            data.AddRange(items);
            return data;
        }
    }
}