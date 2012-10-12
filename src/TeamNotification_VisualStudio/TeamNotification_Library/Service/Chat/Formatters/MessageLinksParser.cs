using System;
using System.Diagnostics;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace TeamNotification_Library.Service.Chat.Formatters
{
    public class MessageLinksParser : IParseMessagesForLinks
    {
        public Span Parse(string message)
        {
            var messageWords = message.Split(' ');
            var finalMessage = new Span();
            foreach (var word in messageWords)
            {
                if (word.Contains("://"))
                {
                    var hyperlink = new Hyperlink(new Run(word)) { NavigateUri = new Uri(word) };
                    hyperlink.RequestNavigate += Hyperlink_RequestNavigateEvent;
                    finalMessage.Inlines.Add(hyperlink);
                }
                else
                {
                    finalMessage.Inlines.Add(new Run(word + " "));
                }
            }

            return finalMessage;
        }

        private void Hyperlink_RequestNavigateEvent(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }
    }
}