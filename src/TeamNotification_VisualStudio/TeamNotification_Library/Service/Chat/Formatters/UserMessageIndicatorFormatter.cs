using System.Windows.Documents;
using TeamNotification_Library.Models;
using TeamNotification_Library.Extensions;

namespace TeamNotification_Library.Service.Chat.Formatters
{
    public class UserMessageIndicatorFormatter : IFormatUserIndicator
    {
        private IFormatDateTime dateTimeFormatter;

        public UserMessageIndicatorFormatter(IFormatDateTime dateTimeFormatter)
        {
            this.dateTimeFormatter = dateTimeFormatter;
        }

        public Bold Get(ChatMessageModel chatMessage)
        {
            var indicator = "{0}({1}):".FormatUsing(chatMessage.UserName, dateTimeFormatter.Format(chatMessage.DateTime));
            return new Bold(new Run(indicator));
        }
    }
}