using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using EnvDTE;
using TeamNotification_Library.Models;
using TeamNotification_Library.Models.UI;

namespace TeamNotification_Library.Service.Controls
{
    public interface IServiceChatRoomsControl
    {
        Collection GetCollection();
        Collection GetMessagesCollection(string roomId);
        void UpdateClipboard(object source, DTE dte);
        void HandlePaste(RichTextBox textBox, DataObjectPastingEventArgs dataObjectPastingEventArgs);
        void SendMessage(MessagesContainer textBox, string roomId);
        void ClearRichTextBox(MessagesContainer textBox);
        void AddReceivedMessage(MessagesContainer messagesContainer, ScrollViewer scrollviewer, string messageData);
        void AddMessages(MessagesContainer messagesContainer, ScrollViewer scrollviewer, string currentRoomId);
    }
}