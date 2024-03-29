﻿using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Async
{
    public class AbstractEventHandler
    {
        protected void Handle<T>(object source, CustomEventHandler<T> handler) where T: new()
        {
            if (handler != null)
                handler(source, new T());
        }

        protected void Handle<T>(object source, CustomEventHandler<T> handler, T args)
        {
            if (handler != null)
                handler(source, args);
        }
    }
}