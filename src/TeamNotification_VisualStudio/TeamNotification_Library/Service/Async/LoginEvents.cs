using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Async
{
    public class LoginEvents : AbstractEventHandler, IHandleLoginEvents
    {
        public event CustomEventHandler<UserHasLogged> UserHasLogged;
        public event CustomEventHandler<UserCouldNotLogIn> UserCouldNotLogIn;
		
        public void OnLoginSuccess(object source, UserHasLogged eventArgs)
        {
            Handle(source, UserHasLogged, eventArgs);
        }

        public void OnLoginFail(object source)
        {
            Handle(source, UserCouldNotLogIn);
        }
    }
}