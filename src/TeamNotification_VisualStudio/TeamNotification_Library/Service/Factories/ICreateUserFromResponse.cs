using System.Collections.Generic;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Factories
{
    public interface ICreateUserFromResponse
    {
        User Get(LoginResponse responseToStore, IEnumerable<CollectionData> items);
    }
}