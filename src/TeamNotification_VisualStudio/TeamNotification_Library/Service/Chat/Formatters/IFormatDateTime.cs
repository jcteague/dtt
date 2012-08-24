using System;

namespace TeamNotification_Library.Service.Chat.Formatters
{
    public interface IFormatDateTime
    {
        string Format(DateTime dateTime);
    }
}