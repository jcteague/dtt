using System.Collections.Generic;
using System.Windows;

namespace TeamNotification_Library.Service.Async.Models
{
    public class SendMessageWasRequested
    {
        public string Message { get; private set; }
        public ResourceDictionary MessageBoxResources { get; private set; }
        public string RoomId { get; private set; }

        public SendMessageWasRequested(string message, ResourceDictionary messageBoxResources, string roomId)
        {
            Message = message;
            MessageBoxResources = messageBoxResources;
            RoomId = roomId;
        }
    }
}