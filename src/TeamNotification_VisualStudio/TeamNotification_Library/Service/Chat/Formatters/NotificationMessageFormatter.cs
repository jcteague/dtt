using System.Windows;
using System.Windows.Documents;
using TeamNotification_Library.Models;
using TeamNotification_Library.Extensions;

namespace TeamNotification_Library.Service.Chat.Formatters
{
    public class NotificationMessageFormatter : IFormatNotificationMessages
    {
        private IParseMessagesForLinks messageLinksParser;

        public NotificationMessageFormatter(IParseMessagesForLinks messageLinksParser)
        {
            this.messageLinksParser = messageLinksParser;
        }

        public Block GetFormattedElement(ChatMessageModel chatMessage)
        {
            var userMessageParagraph = new Paragraph { KeepTogether = true, LineHeight = 1.0, Margin = new Thickness(0, 0, 0, 0) };
            userMessageParagraph.Inlines.Add(messageLinksParser.Parse(chatMessage.chatMessageBody.message.FormatUsing(chatMessage.chatMessageBody.repository_url, chatMessage.chatMessageBody.url)));

            return userMessageParagraph;
        }
    }
}