using AurelienRibon.Ui.SyntaxHighlightBox;

namespace TeamNotification_Library.Service.Factories.UI.Highlighters
{
    public interface ICreateSyntaxHighlightBox<T>
    {
        T Get(string message, int programmingLanguage);
    }
}