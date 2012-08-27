using System.Text.RegularExpressions;
using System.Windows;
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

        public Maybe<Paragraph> GetFormattedElement(ChatMessageModel chatMessage, int lastUserThatInserted)
        {
            var user = "";
            if (lastUserThatInserted != chatMessage.UserId)
            {
                user = chatMessage.UserName;
            }
            var paragraph = new Paragraph {KeepTogether = true, LineHeight = 1.0, Margin = new Thickness(0, 0, 0, 0)};
            paragraph.Inlines.Add(new Bold(new Run(user)));

            0.RangeTo(GetNumberOfLines(chatMessage.Message) - 1).Each(x => paragraph.Inlines.Add(new LineBreak()));

//            return new Maybe<Paragraph>(new Paragraph(new Bold(new Run(user))) { KeepTogether = true, LineHeight = 1.0, Margin = new Thickness(0, 0, 0, 0) });

            return new Maybe<Paragraph>(paragraph);
        }

        private int GetNumberOfLines(string message)
        {
            return message.Split("\r\n").Length;
        }
    }
}