using AurelienRibon.Ui.SyntaxHighlightBox;

namespace TeamNotification_Library.Service.Highlighters
{
    public interface IProvideSyntaxHighlighter
    {
        IHighlighter GetFor(int programmingLanguageIdentifier);
    }
}