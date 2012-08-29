using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Chat.Formatters
{
    public class UserMessageIndicatorFormatter : IFormatUserIndicator
    {
        public Paragraph GetFormattedElement(ChatMessageModel chatMessage, int lastUserThatInserted)
        {
            var user = "";
            if (lastUserThatInserted != chatMessage.UserId)
            {
                user = chatMessage.UserName;
            }
            var paragraph = new Paragraph {KeepTogether = true, LineHeight = 1.0, Margin = new Thickness(0, 0, 0, 0)};
            paragraph.Inlines.Add(new Bold(new Run(user)));

            return paragraph;
        }
    }
}