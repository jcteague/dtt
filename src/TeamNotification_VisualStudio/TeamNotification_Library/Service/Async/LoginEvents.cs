using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Async
{
    public class LoginEvents : AbstractEventHandler, IHandleLoginEvents
    {
        public event CustomEventHandler<UserAttemptedLogin> UserHasLogged;
        public event CustomEventHandler<UserAttemptedLogin> UserCouldNotLogIn;
        
        public void OnLoginSuccess(object source)
        {
            Handle(source, UserHasLogged);
        }

        public void OnLoginFail(object source)
        {
            Handle(source, UserCouldNotLogIn);
        }
        
        private void Handle(object source, CustomEventHandler<UserAttemptedLogin> handler)
        {
            if (handler != null)
                handler(source, new UserAttemptedLogin());
        }
    }
}