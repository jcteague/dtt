using System.Windows.Documents;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Chat.Formatters
{
    public interface IFormatNotificationMessages
    {
        Block GetFormattedElement(ChatMessageModel chatMessage);
    }
}