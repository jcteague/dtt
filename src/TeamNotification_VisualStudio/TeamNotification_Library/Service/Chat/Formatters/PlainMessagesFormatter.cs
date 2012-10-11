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
        private void Hyperlink_RequestNavigateEvent(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }
        public Block GetFormattedElement(ChatMessageModel chatMessage)
        {
            var userMessageParagraph = new Paragraph { KeepTogether = true, LineHeight = 1.0, Margin = new Thickness(0, 0, 0, 0) };


            var messageWords = chatMessage.chatMessageBody.message.Split(' ');
            var finalMessage = new Span();
            foreach(var word in messageWords)
            {
                if (word.Contains("://"))
                {
                    var hyperlink = new Hyperlink(new Run(word)) {NavigateUri = new Uri(word)};
                    hyperlink.RequestNavigate += Hyperlink_RequestNavigateEvent;
                    finalMessage.Inlines.Add(hyperlink);
                }
                else
                {
                    finalMessage.Inlines.Add(new Run(word+" "));
                }
            }
            //userMessageParagraph.Inlines.Add(new Run(chatMessage.chatMessageBody.message));
            userMessageParagraph.Inlines.Add(finalMessage);

            return userMessageParagraph;
        }
    }
}