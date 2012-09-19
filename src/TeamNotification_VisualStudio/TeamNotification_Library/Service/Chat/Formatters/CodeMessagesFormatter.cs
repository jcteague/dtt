using System.Windows;
using System.Windows.Documents;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.Factories.UI;

namespace TeamNotification_Library.Service.Chat.Formatters
{
    public class CodeMessagesFormatter : IFormatCodeMessages
    {
        private IHandleCodePaste codePasteEvents;
        private ICreateSyntaxHighlightBox syntaxHighlightBoxFactory;

        public CodeMessagesFormatter(IHandleCodePaste codePasteEvents, ICreateSyntaxHighlightBox syntaxHighlightBoxFactory)
        {
            this.codePasteEvents = codePasteEvents;
            this.syntaxHighlightBoxFactory = syntaxHighlightBoxFactory;
        }

        public Paragraph GetFormattedElement(ChatMessageModel chatMessage)
        {
            var link = new Hyperlink(new Run("{0} \\ {1} - Line: {2}".FormatUsing(chatMessage.chatMessageBody.project, chatMessage.chatMessageBody.document, chatMessage.chatMessageBody.line.ToString()))) { IsEnabled = true, CommandParameter = chatMessage };
            link.Click += codePasteEvents.OnCodePasteClick;
            var paragraph = new Paragraph(link);
            var box = syntaxHighlightBoxFactory.Get(chatMessage.chatMessageBody.message, chatMessage.chatMessageBody.programminglanguage);
            paragraph.Inlines.Add(new LineBreak());
            paragraph.Inlines.Add(new InlineUIContainer(box) { Resources = chatMessage.AsResources() });

            return paragraph;
        }
    }
}