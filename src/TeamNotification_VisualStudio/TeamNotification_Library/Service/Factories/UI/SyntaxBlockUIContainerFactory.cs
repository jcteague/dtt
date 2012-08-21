using System.Windows.Documents;
using AurelienRibon.Ui.SyntaxHighlightBox;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Highlighters;

namespace TeamNotification_Library.Service.Factories.UI
{
    public class SyntaxBlockUIContainerFactory : ICreateSyntaxBlockUIInstances
    {
        private readonly IProvideSyntaxHighlighter syntaxHighlighterProvider;
        
        public SyntaxBlockUIContainerFactory(IProvideSyntaxHighlighter syntaxHighlighterProvider)
        {
            this.syntaxHighlighterProvider = syntaxHighlighterProvider;
        }

        public BlockUIContainer Get(CodeClipboardData clipboardData)
        {
            return new BlockUIContainer(
                new SyntaxHighlightBox
                {
                    Text = clipboardData.message,
                    CurrentHighlighter = syntaxHighlighterProvider.GetFor(clipboardData.programmingLanguage)
                }) { Resources = clipboardData.AsResources() };
        }
    }
}