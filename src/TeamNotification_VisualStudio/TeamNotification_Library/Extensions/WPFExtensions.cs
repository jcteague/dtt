using System.Windows.Documents;
using EnvDTE;
using TeamNotification_Library.Configuration;
using TextRange = System.Windows.Documents.TextRange;

namespace TeamNotification_Library.Extensions
{
    public static class WPFExtensions
    {
        public static string GetText<T>(this T content) where T : TextElement
        {
            return new TextRange(content.ContentStart, content.ContentEnd).Text;
        }

        public static bool IsPluginWindow(this Window window)
        {
            return window.ObjectKind.ToLower() == "{" + GlobalConstants.Guids.LoginWindowPersistanceString + "}";
        }
    }
}