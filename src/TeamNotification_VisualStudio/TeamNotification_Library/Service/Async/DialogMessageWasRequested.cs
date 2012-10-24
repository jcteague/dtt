using System;

namespace TeamNotification_Library.Service.Async
{
    public class DialogMessageWasRequested
    {
        public string Message { get; set; }

        public Action OkAction { get; set; }

        public Action CancelAction { get; set; }
    }
}