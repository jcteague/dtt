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

        public static bool IsStartPageWindow(this Window window)
        {
            return window.ObjectKind.ToLower() == "{387CB18D-6153-4156-9257-9AC3F9207BBE}".ToLower();
        }
    }
}