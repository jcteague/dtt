using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Xml.Linq;
using AurelienRibon.Ui.SyntaxHighlightBox;

namespace TeamNotification_Library.Service.Highlighters
{
    public class SampleHighlighter
    {
//        private class XmlHighlighter : IHighlighter
//        {
//            private List<HighlighterManager.HighlightWordsRule> wordsRules;
//            private List<HighlighterManager.HighlightLineRule> lineRules;
//            private List<HighlighterManager.AdvancedHighlightRule> regexRules;
//
//            public XmlHighlighter(XElement root)
//            {
//                wordsRules = new List<HighlighterManager.HighlightWordsRule>();
//                lineRules = new List<HighlighterManager.HighlightLineRule>();
//                regexRules = new List<HighlighterManager.AdvancedHighlightRule>();
//
//                foreach (XElement elem in root.Elements())
//                {
//                    switch (elem.Name.ToString())
//                    {
//                        case "HighlightWordsRule": wordsRules.Add(new HighlighterManager.HighlightWordsRule(elem)); break;
//                        case "HighlightLineRule": lineRules.Add(new HighlighterManager.HighlightLineRule(elem)); break;
//                        case "AdvancedHighlightRule": regexRules.Add(new HighlighterManager.AdvancedHighlightRule(elem)); break;
//                    }
//                }
//            }
//
//            public int Highlight(FormattedText text, int previousBlockCode)
//            {
//                //
//                // WORDS RULES
//                //
//                Regex wordsRgx = new Regex("[a-zA-Z_][a-zA-Z0-9_]*");
//                foreach (Match m in wordsRgx.Matches(text.Text))
//                {
//                    foreach (HighlighterManager.HighlightWordsRule rule in wordsRules)
//                    {
//                        foreach (string word in rule.Words)
//                        {
//                            if (rule.Options.IgnoreCase)
//                            {
//                                if (m.Value.Equals(word, StringComparison.InvariantCultureIgnoreCase))
//                                {
//                                    text.SetForegroundBrush(rule.Options.Foreground, m.Index, m.Length);
//                                    text.SetFontWeight(rule.Options.FontWeight, m.Index, m.Length);
//                                    text.SetFontStyle(rule.Options.FontStyle, m.Index, m.Length);
//                                }
//                            }
//                            else
//                            {
//                                if (m.Value == word)
//                                {
//                                    text.SetForegroundBrush(rule.Options.Foreground, m.Index, m.Length);
//                                    text.SetFontWeight(rule.Options.FontWeight, m.Index, m.Length);
//                                    text.SetFontStyle(rule.Options.FontStyle, m.Index, m.Length);
//                                }
//                            }
//                        }
//                    }
//                }
//
//                //
//                // REGEX RULES
//                //
//                foreach (HighlighterManager.AdvancedHighlightRule rule in regexRules)
//                {
//                    Regex regexRgx = new Regex(rule.Expression);
//                    foreach (Match m in regexRgx.Matches(text.Text))
//                    {
//                        text.SetForegroundBrush(rule.Options.Foreground, m.Index, m.Length);
//                        text.SetFontWeight(rule.Options.FontWeight, m.Index, m.Length);
//                        text.SetFontStyle(rule.Options.FontStyle, m.Index, m.Length);
//                    }
//                }
//
//                //
//                // LINES RULES
//                //
//                foreach (HighlighterManager.HighlightLineRule rule in lineRules)
//                {
//                    Regex lineRgx = new Regex(Regex.Escape(rule.LineStart) + ".*");
//                    foreach (Match m in lineRgx.Matches(text.Text))
//                    {
//                        text.SetForegroundBrush(rule.Options.Foreground, m.Index, m.Length);
//                        text.SetFontWeight(rule.Options.FontWeight, m.Index, m.Length);
//                        text.SetFontStyle(rule.Options.FontStyle, m.Index, m.Length);
//                    }
//                }
//
//                return -1;
//            }
//        }
//
//        /// <summary>
//        /// A set of words and their RuleOptions.
//        /// </summary>
//        private class HighlightWordsRule
//        {
//            public List<string> Words { get; private set; }
//            public HighlighterManager.RuleOptions Options { get; private set; }
//
//            public HighlightWordsRule(XElement rule)
//            {
//                Words = new List<string>();
//                Options = new HighlighterManager.RuleOptions(rule);
//
//                string wordsStr = rule.Element("Words").Value;
//                string[] words = Regex.Split(wordsStr, "\\s+");
//
//                foreach (string word in words)
//                    if (!string.IsNullOrWhiteSpace(word))
//                        Words.Add(word.Trim());
//            }
//        }
//
//        /// <summary>
//        /// A line start definition and its RuleOptions.
//        /// </summary>
//        private class HighlightLineRule
//        {
//            public string LineStart { get; private set; }
//            public HighlighterManager.RuleOptions Options { get; private set; }
//
//            public HighlightLineRule(XElement rule)
//            {
//                LineStart = rule.Element("LineStart").Value.Trim();
//                Options = new HighlighterManager.RuleOptions(rule);
//            }
//        }
//
//        /// <summary>
//        /// A regex and its RuleOptions.
//        /// </summary>
//        private class AdvancedHighlightRule
//        {
//            public string Expression { get; private set; }
//            public HighlighterManager.RuleOptions Options { get; private set; }
//
//            public AdvancedHighlightRule(XElement rule)
//            {
//                Expression = rule.Element("Expression").Value.Trim();
//                Options = new HighlighterManager.RuleOptions(rule);
//            }
//        } 
    }
}