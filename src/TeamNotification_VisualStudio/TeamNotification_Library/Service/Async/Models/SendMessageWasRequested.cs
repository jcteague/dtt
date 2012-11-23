using System.Collections.Generic;
using System.Windows;
using ICSharpCode.AvalonEdit.Utils;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Async.Models
{
    public class SendMessageWasRequested
    {
        //public string Message { get; private set; }
        //public ResourceDictionary MessageBoxResources { get; private set; }
        public string RoomId { get; private set; }
        public IEnumerable<ChatMessageBody> Messages { get; private set; }

        //public SendMessageWasRequested(string message, ResourceDictionary messageBoxResources, string roomId)
        //{
        //    Message = message;
        //    MessageBoxResources = messageBoxResources;
        //    RoomId = roomId;
        //}
        public SendMessageWasRequested(string message, string roomId)
        {
            Messages = new List<ChatMessageBody> {new ChatMessageBody {message = message}};
            this.RoomId = roomId;
        }

        public SendMessageWasRequested(IEnumerable<ChatMessageBody> messages, string roomId)
        {
            this.RoomId = roomId;
            this.Messages = messages;
        }
    }
}