using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows;
using System.Text.RegularExpressions;
using AurelienRibon.Ui.SyntaxHighlightBox;

namespace TeamNotification_Library.Service.Highlighters
{
    public class ExampleCSharp : IHighlighter
    {
        private List<HighlightWordsRule> wordsRules;

        public ExampleCSharp()
        {
            wordsRules = new List<HighlightWordsRule>();
            wordsRules.Add(new HighlightWordsRule());
        }

        public int Highlight(FormattedText text, int previousBlockCode)
        {
            //
            // WORDS RULES
            //
            Regex wordsRgx = new Regex("[a-zA-Z_][a-zA-Z0-9_]*");
            foreach (Match m in wordsRgx.Matches(text.Text))
            {
                foreach (HighlightWordsRule rule in wordsRules)
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
            }

            return -1;
        }
    }
}