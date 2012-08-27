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

        public CodeMessagesFormatter(ICreateSyntaxBlockUIInstances syntaxBlockUIContainerFactory)
        {
            this.syntaxBlockUIContainerFactory = syntaxBlockUIContainerFactory;
        }

        public Maybe<Block> GetFormattedElement(ChatMessageModel chatMessage)
        {
            var codeClipboardData = new CodeClipboardData
            {
                message = chatMessage.Message,
                solution = chatMessage.Solution,
                document = chatMessage.Document,
                line = chatMessage.Line,
                column = chatMessage.Column,
                programmingLanguage = chatMessage.ProgrammingLanguage
            };
            return new Maybe<Block>(syntaxBlockUIContainerFactory.Get(codeClipboardData));
        }
    }
}