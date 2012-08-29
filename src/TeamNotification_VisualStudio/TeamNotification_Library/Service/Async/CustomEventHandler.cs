using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Async
{
    public delegate void CustomEventHandler<T>(object sender, T e);
}