using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TeamNotification_Library.Models;
using TeamNotification_Library.Models.UI;
using TeamNotification_Library.Service.Async.Models;
using TeamNotification_Library.UI.Avalon;

namespace TeamNotification_Library.Service.Controls
{
    public interface IServiceChatRoomsControl
    {
        Collection GetCollection();
        Collection GetMessagesCollection(string roomId);
//        void HandlePaste(RichTextBox textBox, IShowCode codeShower, DataObjectPastingEventArgs pasteData);
        void HandlePaste(MixedTextEditor textBox, IShowCode codeShower, DataWasPasted pasteData);
        void SendMessages(IEnumerable<ChatMessageBody> messages, string roomId);
        void ResetContainer(ChatUIElements textBox);
        ChatMessageModel AddReceivedMessage(ChatUIElements messagesContainer, ScrollViewer scrollviewer, string messageData);
        void AddMessages(ChatUIElements messagesContainer, ScrollViewer scrollviewer, string currentRoomId);
        ChatRoomInvitation AddInvitedRoom(ChatUIElements messagesContainer, string messageData);
        void HandleDock(ChatUIElements chatUIElements);
        void LogoutUser(object sender);
    }
}