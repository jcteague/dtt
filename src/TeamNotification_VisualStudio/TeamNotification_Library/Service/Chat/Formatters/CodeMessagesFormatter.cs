using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using AurelienRibon.Ui.SyntaxHighlightBox;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.Async.Models;
using TeamNotification_Library.Service.Factories.UI;

namespace TeamNotification_Library.Service.Chat.Formatters
{
    public class CodeMessagesFormatter : IFormatCodeMessages
    {
        private IHandleCodePaste codePasteEvents;
        private ICreateSyntaxBlockUIInstances syntaxBlockUIContainerFactory;

        public CodeMessagesFormatter(ICreateSyntaxBlockUIInstances syntaxBlockUIContainerFactory, IHandleCodePaste codePasteEvents)
        {
            this.syntaxBlockUIContainerFactory = syntaxBlockUIContainerFactory;
            this.codePasteEvents = codePasteEvents;
        }

//        myFlowDoc.IsEnabled = true;
//                    myFlowDoc.IsHyphenationEnabled = true;
//
//                    var pasteLink = new Hyperlink(new Run("Paste code to: {0} - {1} - {2}".FormatUsing(message.solution, message.project, message.document))) { IsEnabled = true, CommandParameter = message };
//                    pasteLink.Click += new RoutedEventHandler(PasteCode);
//                    userMessageParagraph.IsHyphenationEnabled = true;
//                    userMessageParagraph.Inlines.Add(new Bold(new Run(username + ": ")));
//                    userMessageParagraph.Inlines.Add(pasteLink);

        public Block GetFormattedElement(ChatMessageModel chatMessage)
        {
            var link = new Hyperlink(new Run("Paste code to: {0} - {1} - {2}".FormatUsing(chatMessage.Solution, chatMessage.Project, chatMessage.Document))) { IsEnabled = true, CommandParameter = chatMessage };
//            link.Click += new RoutedEventHandler(PasteCode);
            
            link.Click += codePasteEvents.OnCodePasteClick;

            var codeClipboardData = new CodeClipboardData
            {
                message = chatMessage.Message,
                project = chatMessage.Project,
                solution = chatMessage.Solution,
                document = chatMessage.Document,
                line = chatMessage.Line,
                column = chatMessage.Column,
                programmingLanguage = chatMessage.ProgrammingLanguage
            };
            var paragrapgh = new Paragraph(link);

            var box = new SyntaxHighlightBox
                {
                    Text = codeClipboardData.message,
                    CurrentHighlighter = HighlighterManager.Instance.Highlighters["cSharp"]
                };
            paragrapgh.Inlines.Add(new LineBreak());
            paragrapgh.Inlines.Add(new InlineUIContainer(box)  { Resources = codeClipboardData.AsResources()});

            return paragrapgh;
//            return syntaxBlockUIContainerFactory.Get(codeClipboardData);
        }
    }
}