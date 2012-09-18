using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;
using TeamNotification_Library.Models;
using TeamNotification_Library.Models.UI;

namespace TeamNotification_Library.Service.Chat
{
    public interface IHandleChatMessages
    {
        TableRowGroup AppendMessage(ChatUIElements messagesContainer, ScrollViewer scrollViewer, ChatMessageModel chatMessage);
        void ResetUser();
    }
}