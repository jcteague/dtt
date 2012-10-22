using System;
using System.Windows;
using System.Windows.Documents;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Chat.Formatters
{
    public class DateTimeFormatter : IFormatDateTime
    {
        public string Format(DateTime dateTime)
        {
            var delta = DateTime.UtcNow - dateTime.ToUniversalTime();
            var seconds = delta.TotalSeconds;
            if (seconds < 60)
            {
                return "just now";
            }
            if (seconds < 120)
            {
                return "a minute ago";
            }
            if (seconds < 3600)
            {
                var n = (seconds / 60.0);
                return "{0} minutes ago".FormatUsing(n.Floor().ToString());
            }
            if (seconds < 86400)
            {
                var n = (seconds / 3600.0);
                return "{0} hours ago".FormatUsing(n.Floor().ToString());
            }

            return dateTime.ToShortDateString();
        }

        public Paragraph GetFormattedElement(ChatMessageModel chatMessage)
        {
            var dateTime = FormatDate(DateTime.Parse(chatMessage.date));
            return new Paragraph(new Bold(new Run(dateTime))) { KeepTogether = true, LineHeight = 1.0, Margin = new Thickness(0, 0, 0, 0) };
        }

        private string FormatDate(DateTime dateTime)
        {
            return IsToday(dateTime) ? dateTime.ToString("hh:mm tt") : dateTime.ToShortDateString();
        }

        private bool IsToday(DateTime dateTime)
        {
            return DateTime.UtcNow.ToShortDateString() == dateTime.ToUniversalTime().ToShortDateString();
        }
    }
}