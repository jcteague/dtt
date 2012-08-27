using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Chat.Formatters
{
    public class PlainMessagesFormatter : IFormatPlainMessages
    {
        public Maybe<Block> GetFormattedElement(ChatMessageModel chatMessage)
        {
            var userMessageParagraph = new Paragraph { KeepTogether = true, LineHeight = 1.0, Margin = new Thickness(0, 0, 0, 0) };
            userMessageParagraph.Inlines.Add(new Run(chatMessage.Message));

            return new Maybe<Block>(userMessageParagraph);
        }
    }
}