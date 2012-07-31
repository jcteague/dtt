using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Providers;

namespace TeamNotification_Library.Service.Controls
{
    public interface IServiceLoginControl
    {
        Collection GetCollection();
        void HandleClick();
    }
}