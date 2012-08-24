using System;
using TeamNotification_Library.Extensions;

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

            return dateTime.ToString("mm/dd/yyyy");
        }
    }
}