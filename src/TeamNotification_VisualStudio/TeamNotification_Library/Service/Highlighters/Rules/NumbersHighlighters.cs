using System.Text.RegularExpressions;
using System.Windows.Media;
using TeamNotification_Library.Service.Factories;

namespace TeamNotification_Library.Service.Highlighters.Rules
{
    public class NumbersHighlighters : IHighlightNumbers
    {
        private HighlightNumbersRule rule;

        public NumbersHighlighters()
        {
            rule = new HighlightNumbersRule();
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