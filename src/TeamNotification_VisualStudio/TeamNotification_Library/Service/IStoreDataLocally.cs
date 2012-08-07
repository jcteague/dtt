using System.Collections.Generic;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service
{
    public interface IStoreDataLocally
    {
        void Store(User user, IEnumerable<CollectionData> items);
        User GetUser();
    }
}