using System.Text.RegularExpressions;
using System.Windows.Media;

namespace TeamNotification_Library.Service.Highlighters.Rules
{
    public class LineCommentsHighlighter : IHighlightLineComments
    {
        private HighlightLineCommentRule rule;

        public LineCommentsHighlighter()
        {
            rule = new HighlightLineCommentRule();
        }

        public int Format(FormattedText text, int previousBlockCode)
        {
            Regex lineRgx = new Regex(Regex.Escape(rule.LineStart) + ".*");
            foreach (Match m in lineRgx.Matches(text.Text))
            {
                text.SetForegroundBrush(rule.Options.Foreground, m.Index, m.Length);
                text.SetFontWeight(rule.Options.FontWeight, m.Index, m.Length);
                text.SetFontStyle(rule.Options.FontStyle, m.Index, m.Length);
            }

            return BlockCodes.Ok;
        }
    }
}