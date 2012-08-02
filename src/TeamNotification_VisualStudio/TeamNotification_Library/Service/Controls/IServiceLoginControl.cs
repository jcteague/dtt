using System.Collections.Generic;
using System.Windows.Controls;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.Providers;

namespace TeamNotification_Library.Service.Controls
{
    public interface IServiceLoginControl
    {
        Collection GetCollection();
        void HandleClick(IEnumerable<CollectionData> items);
        bool IsUserLogged();

        event CustomEventHandler UserHasLogged;
        event CustomEventHandler UserCouldNotLogIn;
    }
}