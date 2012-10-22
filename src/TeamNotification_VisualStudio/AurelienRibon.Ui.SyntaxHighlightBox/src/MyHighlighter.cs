using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

namespace AurelienRibon.Ui.SyntaxHighlightBox
{
    public class MyHighlighter : IHighlighter
    {
        private List<HighlightWordsRule> wordsRules;

        public MyHighlighter()
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

        /// <summary>
        /// A set of words and their RuleOptions.
        /// </summary>
        private class HighlightWordsRule
        {
            public List<string> Words { get; private set; }
            public RuleOptions Options { get; private set; }

            public HighlightWordsRule()
            {
                Words = new List<string>();
                Options = new RuleOptions("#FF00FF", "Bold", "Normal");

                string[] words = ruleWords.ToArray();

                foreach (string word in words)
                    if (!string.IsNullOrWhiteSpace(word))
                        Words.Add(word.Trim());
            }
        }

        /// <summary>
        /// A set of options liked to each rule.
        /// </summary>
        private class RuleOptions
        {
            public bool IgnoreCase { get; private set; }
            public Brush Foreground { get; private set; }
            public FontWeight FontWeight { get; private set; }
            public FontStyle FontStyle { get; private set; }

            public RuleOptions(string foreground, string fontWeight, string fontStyle)
            {
                string foregroundStr = foreground.Trim();
                string fontWeightStr = fontWeight.Trim();
                string fontStyleStr = fontStyle.Trim();

                Foreground = (Brush)new BrushConverter().ConvertFrom(foregroundStr);
                FontWeight = (FontWeight)new FontWeightConverter().ConvertFrom(fontWeightStr);
                FontStyle = (FontStyle)new FontStyleConverter().ConvertFrom(fontStyleStr);
            }
        }

        private static List<string> ruleWords = new List<string> {
                                                          "abstract",
                                                          "as",
                                                          "base",
                                                          "bool",
                                                          "break",
                                                          "byte",
                                                          "case",
                                                          "catch",
                                                          "char",
                                                          "checked",
                                                          "class",
                                                          "const",
                                                          "continue",
                                                          "decimal",
                                                          "default",
                                                          "delegate",
                                                          "do",
                                                          "double",
                                                          "else",
                                                          "enum",
                                                          "event",
                                                          "explicit",
                                                          "extern",
                                                          "false",
                                                          "finally",
                                                          "fixed",
                                                          "float",
                                                          "for",
                                                          "foreach",
                                                          "goto",
                                                          "if",
                                                          "implicit",
                                                          "in",
                                                          "int",
                                                          "interface",
                                                          "internal",
                                                          "is",
                                                          "lock",
                                                          "long",
                                                          "namespace",
                                                          "new",
                                                          "null",
                                                          "object",
                                                          "operator",
                                                          "out",
                                                          "override",
                                                          "params",
                                                          "private",
                                                          "protected",
                                                          "public",
                                                          "readonly",
                                                          "ref",
                                                          "return",
                                                          "sbyte",
                                                          "sealed",
                                                          "short",
                                                          "sizeof",
                                                          "stackalloc",
                                                          "static",
                                                          "string",
                                                          "struct",
                                                          "switch",
                                                          "this",
                                                          "throw",
                                                          "true",
                                                          "try",
                                                          "typeof",
                                                          "uint",
                                                          "ulong",
                                                          "unchecked",
                                                          "unsafe",
                                                          "ushort",
                                                          "using",
                                                          "virtual",
                                                          "void",
                                                          "volatile",
                                                          "while",
                                                          "var"
                                                      };
    }
}