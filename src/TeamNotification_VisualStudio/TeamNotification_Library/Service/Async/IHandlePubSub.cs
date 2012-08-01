using System;

namespace TeamNotification_Library.Service.Async
{
    public interface IHandlePubSub
    {
        void Publish<T>(T source, Func<T, CustomEventHandler> getter);
    }

    public class PubSubService : IHandlePubSub
    {
        public void Publish<T>(T source, Func<T, CustomEventHandler> getter)
        {
//            HandleEvent(getter(source));
        }
    }
}