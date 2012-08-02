using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service
{
    public class LocalDataStorageService : IStoreDataLocally
    {
        public User User { get; set; }

        public LocalDataStorageService()
        {
            User = new User();
        }

        public void Store(User userToStore)
        {
            User = userToStore;
        }
    }
}