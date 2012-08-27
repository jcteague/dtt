using System.Windows.Controls;
using TeamNotification_Library.Models;
using TeamNotification_Library.Models.UI;

namespace TeamNotification_Library.Service.Chat
{
    public interface IHandleChatMessages
    {
        void AppendMessage(MessagesContainer messageList, ScrollViewer scrollViewer, ChatMessageModel chatMessage);
    }
}