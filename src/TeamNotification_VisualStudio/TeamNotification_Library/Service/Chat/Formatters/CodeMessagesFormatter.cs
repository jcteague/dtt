using System.Windows;
using System.Windows.Documents;
using ICSharpCode.AvalonEdit;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.Factories.UI;
using TeamNotification_Library.Service.Factories.UI.Highlighters;

namespace TeamNotification_Library.Service.Chat.Formatters
{
    public class CodeMessagesFormatter : IFormatCodeMessages
    {
        private IHandleCodePaste codePasteEvents;
        private ICreateSyntaxHighlightBox<TextEditor> syntaxHighlightBoxFactory;

        public CodeMessagesFormatter(IHandleCodePaste codePasteEvents, ICreateSyntaxHighlightBox<TextEditor> syntaxHighlightBoxFactory)
        {
            this.codePasteEvents = codePasteEvents;
            this.syntaxHighlightBoxFactory = syntaxHighlightBoxFactory;
        }

        public Paragraph GetFormattedElement(ChatMessageModel chatMessage)
        {
            var title = new Bold(new Run("{0} \\ {1} - Line: {2} ".FormatUsing(chatMessage.chatMessageBody.project, chatMessage.chatMessageBody.document, chatMessage.chatMessageBody.line.ToString())));
            var pasteCodeLink = new Hyperlink(new Run("Paste into file")) { IsEnabled = true, CommandParameter = chatMessage };
            var gotoFileLink = new Hyperlink(new Run("Go to line")) { IsEnabled = true, CommandParameter = chatMessage };
            
            //link.ToolTip = "Paste this code on file {0} \\ {1}, at line {2}".FormatUsing(chatMessage.chatMessageBody.project, chatMessage.chatMessageBody.document, chatMessage.chatMessageBody.line.ToString());
            pasteCodeLink.Click += codePasteEvents.OnCodePasteClick;
            gotoFileLink.Click += codePasteEvents.OnCodeQuotedClick;
            var paragraph = new Paragraph(title);
            paragraph.Inlines.Add(pasteCodeLink);
            paragraph.Inlines.Add(new Run(" - "));
            paragraph.Inlines.Add(gotoFileLink);
            var box = syntaxHighlightBoxFactory.Get(chatMessage.chatMessageBody.message, chatMessage.chatMessageBody.programminglanguage);
            paragraph.Inlines.Add(new LineBreak());
            paragraph.Inlines.Add(new InlineUIContainer(box) { Resources = chatMessage.AsResources() });

            return paragraph;
        }
    }
}