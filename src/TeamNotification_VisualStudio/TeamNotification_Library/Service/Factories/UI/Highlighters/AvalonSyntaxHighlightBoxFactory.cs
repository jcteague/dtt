using System.Windows;
using System.Windows.Controls;
using AurelienRibon.Ui.SyntaxHighlightBox;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using TeamNotification_Library.Service.Highlighters;

namespace TeamNotification_Library.Service.Factories.UI.Highlighters
{
    public class AvalonSyntaxHighlightBoxFactory : ICreateSyntaxHighlightBox<TextEditor>
    {
        private readonly IProvideSyntaxHighlighter<IHighlightingDefinition> syntaxHighlighterProvider;

        public AvalonSyntaxHighlightBoxFactory(IProvideSyntaxHighlighter<IHighlightingDefinition> syntaxHighlighterProvider)
        {
            this.syntaxHighlighterProvider = syntaxHighlighterProvider;
        }

        public TextEditor Get(string message, int programmingLanguage)
        {
            return new TextEditor
            {
                Text = message,
                SyntaxHighlighting = syntaxHighlighterProvider.GetFor(programmingLanguage),
                BorderThickness = new Thickness(0),
                IsReadOnly = true,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden,
                VerticalScrollBarVisibility = ScrollBarVisibility.Hidden
            };
        }
    }
}