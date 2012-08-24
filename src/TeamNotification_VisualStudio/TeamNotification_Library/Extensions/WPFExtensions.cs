using System.Windows.Documents;

namespace TeamNotification_Library.Extensions
{
    public static class WPFExtensions
    {
        public static string GetText(this Paragraph paragraph)
        {
            return new TextRange(paragraph.ContentStart, paragraph.ContentEnd).Text;
        }

        // TODO: This should just be a Generic
        public static string GetText(this Bold content)
        {
            return new TextRange(content.ContentStart, content.ContentEnd).Text;
        }
    }
}