using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Providers
{
    public class UserProvider : IProvideUser
    {
        private readonly IStoreDataLocally localStorageService;

        public UserProvider(IStoreDataLocally localStorageService)
        {
            this.localStorageService = localStorageService;
        }

        public User GetUser()
        {
            return localStorageService.GetUser();
        }

        public bool IsLogged()
        {
            return GetUser() != null;
        }
    }
}