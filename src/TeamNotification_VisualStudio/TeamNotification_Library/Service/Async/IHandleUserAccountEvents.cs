using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Async
{
    public interface IHandleUserAccountEvents
    {
        event CustomEventHandler<UserHasLogged> UserHasLogged;
        event CustomEventHandler<UserCouldNotLogIn> UserCouldNotLogIn;
        event CustomEventHandler<UserHasLogout> UserHasLogout;

        void OnLoginSuccess(object source, UserHasLogged eventArgs);
        void OnLoginFail(object source);
        void OnLogout(object source, UserHasLogout eventArgs);
        void Clear();
    }
}