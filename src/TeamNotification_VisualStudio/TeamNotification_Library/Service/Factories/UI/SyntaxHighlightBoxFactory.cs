using System.Windows;
using AurelienRibon.Ui.SyntaxHighlightBox;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Highlighters;

namespace TeamNotification_Library.Service.Factories.UI
{
    public class SyntaxHighlightBoxFactory : ICreateSyntaxHighlightBox
    {
        private readonly IProvideSyntaxHighlighter syntaxHighlighterProvider;

        public SyntaxHighlightBoxFactory(IProvideSyntaxHighlighter syntaxHighlighterProvider)
        {
            this.syntaxHighlighterProvider = syntaxHighlighterProvider;
        }

        public SyntaxHighlightBox Get(string message, int programmingLanguage)
        {
            return new SyntaxHighlightBox
                {
                    Text = message,
                    CurrentHighlighter = syntaxHighlighterProvider.GetFor(programmingLanguage),
                    BorderThickness = new Thickness(0)
                };
        }
    }
}