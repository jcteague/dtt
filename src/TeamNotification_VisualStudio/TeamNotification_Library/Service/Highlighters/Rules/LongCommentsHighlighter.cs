using System;
using System.Linq;
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
            string textStr = text.Text;
            if (previousBlockCode == BlockCodes.LongCommentStarted)
            {
                FormatText(text, 0, textStr.Length);
                return BlockCodes.LongCommentStarted;
            }
            
            var indexOfStartComment = textStr.IndexOf("/*");
            if (indexOfStartComment != -1)
            {
                FormatText(text, indexOfStartComment, textStr.Length - indexOfStartComment);
                return BlockCodes.LongCommentStarted;    
            }

            var indexOfEndComment = textStr.IndexOf("*/");
            if (indexOfEndComment != -1)
            {
                FormatText(text, 0, indexOfEndComment);
                return BlockCodes.LongCommentStarted;    
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