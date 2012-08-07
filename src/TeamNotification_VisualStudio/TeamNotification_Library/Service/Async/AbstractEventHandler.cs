namespace TeamNotification_Library.Service.Async
{
    public class AbstractEventHandler
    {
        protected void Handle(object source, CustomEventHandler handler)
        {
            if (handler != null)
                handler(source, new CustomEventArgs());
        }

        protected void Handle(object source, CustomEventHandler handler, CustomEventArgs args)
        {
            if (handler != null)
                handler(source, args);
        } 
    }
}