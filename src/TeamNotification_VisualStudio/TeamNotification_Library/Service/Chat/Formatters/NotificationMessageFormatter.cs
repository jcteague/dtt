using System.Windows;
using System.Windows.Documents;
using TeamNotification_Library.Models;
using TeamNotification_Library.Extensions;

namespace TeamNotification_Library.Service.Chat.Formatters
{
    public class NotificationMessageFormatter : IFormatNotificationMessages
    {
        public Block GetFormattedElement(ChatMessageModel chatMessage)
        {
            var userMessageParagraph = new Paragraph { KeepTogether = true, LineHeight = 1.0, Margin = new Thickness(0, 0, 0, 0) };
            userMessageParagraph.Inlines.Add(new Run(chatMessage.chatMessageBody.message.FormatUsing(chatMessage.chatMessageBody.repository_url, chatMessage.chatMessageBody.url)));

            return userMessageParagraph;
        }
    }
}