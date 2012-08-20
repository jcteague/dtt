using System;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace TeamNotification_Library.Service.Highlighters.Rules
{
    public class WordsHighlighter : IHighlightWords
    {
        private HighlightWordsRule rule;

        public WordsHighlighter()
        {
            // Due to some strange behavior in the Syntax Formatter, this rule cannot be created using the factory
            rule = new HighlightWordsRule();
        }

        public int Format(FormattedText text, int previousBlockCode)
        {
            Regex wordsRgx = new Regex("[a-zA-Z_][a-zA-Z0-9_]*");
            foreach (Match m in wordsRgx.Matches(text.Text))
            {
                foreach (string word in rule.Words)
                {
                    if (rule.Options.IgnoreCase)
                    {
                        if (m.Value.Equals(word, StringComparison.InvariantCultureIgnoreCase))
                        {
                            text.SetForegroundBrush(rule.Options.Foreground, m.Index, m.Length);
                            text.SetFontWeight(rule.Options.FontWeight, m.Index, m.Length);
                            text.SetFontStyle(rule.Options.FontStyle, m.Index, m.Length);
                        }
                    }
                    else
                    {
                        if (m.Value == word)
                        {
                            text.SetForegroundBrush(rule.Options.Foreground, m.Index, m.Length);
                            text.SetFontWeight(rule.Options.FontWeight, m.Index, m.Length);
                            text.SetFontStyle(rule.Options.FontStyle, m.Index, m.Length);
                        }
                    }
                }
            }

            return 0;
        }
    }
}