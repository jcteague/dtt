using System;

namespace TeamNotification_Library.Service.Async
{
    public interface IRunInBackgroundWorker
    {
        void Run(Action action);
    }
}