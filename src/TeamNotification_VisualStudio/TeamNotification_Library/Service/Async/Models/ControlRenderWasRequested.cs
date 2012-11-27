using System;

namespace TeamNotification_Library.Service.Async.Models
{
    public class ControlRenderWasRequested
    {
        public Type Control { get; private set; }
        public object ControlData { get; private set; }
        public Action<object> Action { get; private set; }

        public ControlRenderWasRequested(Type control, object controlData, Action<object> action = null)
        {
            Control = control;
            ControlData = controlData;
            Action = action;
        }
    }
}