using System.Windows;
using AurelienRibon.Ui.SyntaxHighlightBox;
using ICSharpCode.AvalonEdit.Highlighting;
using TeamNotification_Library.Service.Highlighters;
using IHighlighter = AurelienRibon.Ui.SyntaxHighlightBox.IHighlighter;

namespace TeamNotification_Library.Service.Factories.UI.Highlighters
{
    public class AurelienRibonSyntaxHighlightBoxFactory : ICreateSyntaxHighlightBox<SyntaxHighlightBox>
    {
        private readonly IProvideSyntaxHighlighter<IHighlighter> syntaxHighlighterProvider;

        public AurelienRibonSyntaxHighlightBoxFactory(IProvideSyntaxHighlighter<IHighlighter> syntaxHighlighterProvider)
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