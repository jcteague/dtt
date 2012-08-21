using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media;
using TeamNotification_Library.Extensions;

namespace TeamNotification_Library.Service.Highlighters.Rules
{
    public class LongCommentsHighlighter : IHighlightLongComments
    {
        private HighlightLongCommentRule rule;

        public LongCommentsHighlighter()
        {
            rule = new HighlightLongCommentRule();
        }

        public int Format(FormattedText text, int previousBlockCode)
        {
//            var textStr = text.Text;
//
//            var indexOfStartComment = textStr.IndexOf("/*");
//            if (indexOfStartComment != -1)
//            {
//                var indexOfEndComment = textStr.IndexOf("*/");
//                if (indexOfEndComment != -1)
//                {
//                    FormatText(text, indexOfStartComment, indexOfEndComment);
//                }
//                else
//                {
//                    FormatText(text, indexOfStartComment, textStr.Length);
//                }
//            }

            Regex regexRgx = new Regex(rule.Expression);
            foreach (Match m in regexRgx.Matches(text.Text))
            {
                text.SetForegroundBrush(rule.Options.Foreground, m.Index, m.Length);
                text.SetFontWeight(rule.Options.FontWeight, m.Index, m.Length);
                text.SetFontStyle(rule.Options.FontStyle, m.Index, m.Length);
            }

            return BlockCodes.Ok;
        }

        private void FormatText(FormattedText text, int start, int length)
        {
            text.SetForegroundBrush(rule.Options.Foreground, start, length);
            text.SetFontWeight(rule.Options.FontWeight, start, length);
            text.SetFontStyle(rule.Options.FontStyle, start, length);
        }
    }
}