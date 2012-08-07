using System.Collections.Generic;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Factories
{
    public interface ICreateUserFromResponse
    {
        User Get(User userToStore, IEnumerable<CollectionData> items);
    }
}