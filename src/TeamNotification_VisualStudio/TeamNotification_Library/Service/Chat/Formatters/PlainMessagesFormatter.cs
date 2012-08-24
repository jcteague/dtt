using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Chat.Formatters
{
    public class PlainMessagesFormatter : IFormatPlainMessages
    {
        private IFormatUserIndicator userIndicatorFormatter;

        public PlainMessagesFormatter(IFormatUserIndicator userIndicatorFormatter)
        {
            this.userIndicatorFormatter = userIndicatorFormatter;
        }

        public IEnumerable<Block> GetFormattedElement(ChatMessageModel chatMessage, int lastUserThatInserted)
        {
            var userMessageParagraph = new Paragraph { KeepTogether = true, LineHeight = 1.0, Margin = new Thickness(0, 0, 0, 0) };
            if (lastUserThatInserted != chatMessage.UserId)
            {
                userMessageParagraph.Inlines.Add(userIndicatorFormatter.Get(chatMessage));
            }
            userMessageParagraph.Inlines.Add(new Run(chatMessage.Message));
            
            return new List<Block> {userMessageParagraph};
        }
    }
}