using System.Text.RegularExpressions;
using System.Windows.Media;

namespace TeamNotification_Library.Service.Highlighters.Rules
{
    public class StringsHighlighter : IHighlightStrings
    {
        private HighlightStringsRule rule;

        public StringsHighlighter()
        {
            rule = new HighlightStringsRule();
        }

        public int Format(FormattedText text, int previousBlockCode)
        {
            Regex regexRgx = new Regex(rule.Expression);
            foreach (Match m in regexRgx.Matches(text.Text))
            {
                text.SetForegroundBrush(rule.Options.Foreground, m.Index, m.Length);
                text.SetFontWeight(rule.Options.FontWeight, m.Index, m.Length);
                text.SetFontStyle(rule.Options.FontStyle, m.Index, m.Length);
            }

            return 0;
        }
    }
}