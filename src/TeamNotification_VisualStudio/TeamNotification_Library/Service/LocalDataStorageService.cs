using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service
{
    public class LocalDataStorageService : IStoreDataLocally
    {
        public User User { get; set; }

        public void Store(User userToStore)
        {
            User = userToStore;
        }
    }
}