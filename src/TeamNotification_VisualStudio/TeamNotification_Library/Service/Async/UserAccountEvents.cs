using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Async
{
    public class UserAccountEvents : AbstractEventHandler, IHandleUserAccountEvents
    {
        public event CustomEventHandler<UserHasLogged> UserHasLogged;
        public event CustomEventHandler<UserCouldNotLogIn> UserCouldNotLogIn;
        public event CustomEventHandler<UserHasLogout> UserHasLogout;

        public void OnLoginSuccess(object source, UserHasLogged eventArgs)
        {
            Handle(source, UserHasLogged, eventArgs);
        }

        public void OnLoginFail(object source)
        {
            Handle(source, UserCouldNotLogIn);
        }

        public void OnLogout(object source, UserHasLogout eventArgs)
        {
            Handle(source, UserHasLogout);
        }

        public void Clear()
        {
            UserHasLogged = null;
            UserCouldNotLogIn = null;
            UserHasLogout = null;
        }
    }
}