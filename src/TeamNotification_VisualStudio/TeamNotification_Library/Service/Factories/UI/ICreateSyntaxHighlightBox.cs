using AurelienRibon.Ui.SyntaxHighlightBox;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Factories.UI
{
    public interface ICreateSyntaxHighlightBox
    {
        SyntaxHighlightBox Get(string message, int programminglanguage);
    }
}