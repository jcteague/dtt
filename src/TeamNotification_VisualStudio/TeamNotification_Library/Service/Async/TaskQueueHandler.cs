using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamNotification_Library.Service.Async
{
    public class TaskQueueHandler : IHandleTasksQueue
    {
        public void Enqueue(IEnumerable<Action> iactions)
        {
            var actions = iactions.ToList();
            var task = Task.Factory.StartNew(actions.First());
            foreach (var action in actions.Skip(1))
            {
                task = task.ContinueWith(tsk => action());
            }
        }

        public void Enqueue(Action action)
        {
            throw new NotImplementedException();
        }
    }
}