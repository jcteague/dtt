using System.Windows.Documents;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Chat.Formatters
{
    public interface IFormatUserIndicator
    {
        Bold Get(ChatMessageModel chatMessage);
    }
}