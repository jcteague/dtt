using System;
using System.Windows.Documents;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Chat.Formatters
{
    public interface IFormatDateTime
    {
        string Format(DateTime dateTime);
        Paragraph GetFormattedElement(ChatMessageModel chatMessage);
    }
}