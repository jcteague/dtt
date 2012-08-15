using System;
using System.Collections.Generic;

namespace TeamNotification_Library.Service.Async
{
    public interface IHandleTasksQueue
    {
        void Enqueue(IEnumerable<Action> actions);
        void Enqueue(Action action);
    }
}