using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Factories.UI;

namespace TeamNotification_Library.Service.Chat.Formatters
{
    public class CodeMessagesFormatter : IFormatCodeMessages
    {
        private ICreateSyntaxBlockUIInstances syntaxBlockUIContainerFactory;
        private IFormatUserIndicator userIndicatorFormatter;

        public CodeMessagesFormatter(ICreateSyntaxBlockUIInstances syntaxBlockUIContainerFactory, IFormatUserIndicator userIndicatorFormatter)
        {
            this.syntaxBlockUIContainerFactory = syntaxBlockUIContainerFactory;
            this.userIndicatorFormatter = userIndicatorFormatter;
        }

        public IEnumerable<Block> GetFormattedElement(ChatMessageModel chatMessage, int lastUserThatInserted)
        {
            var blocks = new List<Block>();
            if (lastUserThatInserted != chatMessage.UserId)
            {
                var userMessageParagraph = new Paragraph { KeepTogether = true, LineHeight = 1.0, Margin = new Thickness(0, 0, 0, 0) };
                userMessageParagraph.Inlines.Add(userIndicatorFormatter.Get(chatMessage));
                blocks.Add(userMessageParagraph);
            }
            
            var codeClipboardData = new CodeClipboardData
            {
                message = chatMessage.Message,
                solution = chatMessage.Solution,
                document = chatMessage.Document,
                line = chatMessage.Line,
                column = chatMessage.Column,
                programmingLanguage = chatMessage.ProgrammingLanguage
            };
            blocks.Add(syntaxBlockUIContainerFactory.Get(codeClipboardData));

            return blocks;
        }
    }
}