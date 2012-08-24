using System.Windows.Documents;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Chat.Formatters
{
    public class UserMessageIndicatorFormatter : IFormatUserIndicator
    {
        public Bold Get(ChatMessageModel chatMessage)
        {
            return new Bold(new Run(chatMessage.UserName + ":"));
        }
    }
}