using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Async
{
    public interface IHandleLoginEvents
    {
        event CustomEventHandler<UserHasLogged> UserHasLogged;
        event CustomEventHandler<UserCouldNotLogIn> UserCouldNotLogIn;

        void OnLoginSuccess(object source, UserHasLogged eventArgs);
        void OnLoginFail(object source);
    }
}