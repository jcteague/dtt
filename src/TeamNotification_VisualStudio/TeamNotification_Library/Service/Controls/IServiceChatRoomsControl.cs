using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using EnvDTE;
using TeamNotification_Library.Models;
using TeamNotification_Library.Models.UI;
using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Controls
{
    public interface IServiceChatRoomsControl
    {
        Collection GetCollection();
        Collection GetMessagesCollection(string roomId);
        void UpdateClipboard(object source, DTE dte);
        void HandlePaste(RichTextBox textBox, DataObjectPastingEventArgs dataObjectPastingEventArgs);
        void SendMessage(RichTextBox textBox, string roomId);
        void ResetContainer(ChatUIElements textBox);
        void AddReceivedMessage(ChatUIElements messagesContainer, ScrollViewer scrollviewer, string messageData);
        void AddMessages(ChatUIElements messagesContainer, ScrollViewer scrollviewer, string currentRoomId);
        void HandleDock(ChatUIElements chatUIElements, ToolWindowWasDocked toolWindowWasDockedArgs);
    }
}