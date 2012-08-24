using System.Windows.Controls;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Chat
{
    public interface IHandleChatMessages
    {
        void AppendMessage(RichTextBox messageList, ScrollViewer scrollViewer, ChatMessageModel chatMessage);
    }
}