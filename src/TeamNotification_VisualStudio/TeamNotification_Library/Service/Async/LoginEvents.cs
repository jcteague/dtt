namespace TeamNotification_Library.Service.Async
{
    public class LoginEvents : IHandleLoginEvents
    {
        public event CustomEventHandler UserHasLogged;
        public event CustomEventHandler UserCouldNotLogIn;
        
        public void OnLoginSuccess(object source)
        {
            Handle(source, UserHasLogged);
        }

        public void OnLoginFail(object source)
        {
            Handle(source, UserCouldNotLogIn);
        }
        
        private void Handle(object source, CustomEventHandler handler)
        {
            if (handler != null)
                handler(source, new CustomEventArgs());
        }
    }
}