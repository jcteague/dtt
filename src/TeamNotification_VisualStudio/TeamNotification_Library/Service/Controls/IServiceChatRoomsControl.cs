using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using EnvDTE;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Controls
{
    public interface IServiceChatRoomsControl
    {
        Collection GetCollection();
        Collection GetMessagesCollection(string roomId);
        void UpdateClipboard(object source, DTE dte);
        void HandlePaste(RichTextBox textBox, DataObjectPastingEventArgs dataObjectPastingEventArgs);
        void SendMessage(RichTextBox textBox, string roomId);
        void ClearRichTextBox(RichTextBox textBox);
        void AddReceivedMessage(RichTextBox messageList, ScrollViewer scrollviewer, string channel, string messageData);
        void AddMessages(RichTextBox messageList, ScrollViewer scrollviewer, string currentRoomId);
    }
}