using System.Collections.Generic;
using System.Linq;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Factories
{
    public class UserFromResponseGetter : ICreateUserFromResponse
    {
        public User Get(User userToStore, IEnumerable<CollectionData> items)
        {
            return new User
            {
                id = userToStore.id,
                email = userToStore.email,
                password = items.First(x => x.name == Globals.Fields.Password).value
            };
        }
    }
}