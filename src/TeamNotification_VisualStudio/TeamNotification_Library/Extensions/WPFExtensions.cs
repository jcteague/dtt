using System.Windows.Documents;

namespace TeamNotification_Library.Extensions
{
    public static class WPFExtensions
    {
        public static string GetText<T>(this T content) where T : TextElement
        {
            return new TextRange(content.ContentStart, content.ContentEnd).Text;
        }
    }
}