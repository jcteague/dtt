using System.Windows;
using System.Windows.Controls;
using TeamNotification_Library.Models;
using TeamNotification_Library.Models.UI;

namespace TeamNotification_Library.Service.Controls
{
    public interface IServiceChatRoomsControl
    {
        Collection GetCollection();
        Collection GetMessagesCollection(string roomId);
        void HandlePaste(RichTextBox textBox, DataObjectPastingEventArgs dataObjectPastingEventArgs);
        void SendMessage(RichTextBox textBox, string roomId);
        void ResetContainer(ChatUIElements textBox);
        ChatMessageModel AddReceivedMessage(ChatUIElements messagesContainer, ScrollViewer scrollviewer, string messageData);
        void AddMessages(ChatUIElements messagesContainer, ScrollViewer scrollviewer, string currentRoomId);
        void HandleDock(ChatUIElements chatUIElements);
        void LogoutUser(object sender);
    }
}