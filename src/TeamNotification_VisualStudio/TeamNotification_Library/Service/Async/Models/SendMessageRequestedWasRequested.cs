using System.Collections.Generic;

namespace TeamNotification_Library.Service.Async.Models
{
    public class SendMessageRequestedWasRequested
    {
        public string Message { get; private set; }
        public SortedList<int, object> MessageBoxContentResource { get; private set; }

        public SendMessageRequestedWasRequested(string message, SortedList<int, object> messageBoxContentResource)
        {
            Message = message;
            MessageBoxContentResource = messageBoxContentResource;
        }
    }
}