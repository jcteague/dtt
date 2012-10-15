using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;
using TeamNotification_Library.Models;
using System.Diagnostics;

namespace TeamNotification_Library.Service.Chat.Formatters
{
    public class PlainMessagesFormatter : IFormatPlainMessages
    {
        private IParseMessagesForLinks messageLinksParser;

        public PlainMessagesFormatter(IParseMessagesForLinks messageLinksParser)
        {
            this.messageLinksParser = messageLinksParser;
        }

        public Block GetFormattedElement(ChatMessageModel chatMessage)
        {
            var userMessageParagraph = new Paragraph { KeepTogether = true, LineHeight = 1.0, Margin = new Thickness(0, 0, 0, 0) };
            userMessageParagraph.Inlines.Add(messageLinksParser.Parse(chatMessage.chatMessageBody.message));

            return userMessageParagraph;
        }
    }
}