using AurelienRibon.Ui.SyntaxHighlightBox;

namespace TeamNotification_Library.Service.Highlighters
{
    public interface IProvideSyntaxHighlighter<T>
    {
        T GetFor(int programminglanguageIdentifier);
    }
}