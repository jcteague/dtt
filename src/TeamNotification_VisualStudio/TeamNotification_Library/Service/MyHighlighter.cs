using System.Windows.Media;
using AurelienRibon.Ui.SyntaxHighlightBox;

namespace TeamNotification_Library.Service
{
    public class MyHighlighter : IHighlighter
    {
        public int Highlight(FormattedText text, int previousBlockCode)
        {
            return 1;
        }
    }
}