using System.Collections.Generic;

namespace TeamNotification_Library.Service.Highlighters.Rules
{
    public class HighlightWordsRule
    {
        public List<string> Words { get; private set; }
        public RuleOptions Options { get; private set; }

        public HighlightWordsRule()
        {
            Words = new List<string>();
            Options = new RuleOptions("#FF0000", "Bold", "Normal");

            string[] words = ruleWords.ToArray();

            foreach (string word in words)
                if (!string.IsNullOrWhiteSpace(word))
                    Words.Add(word.Trim());
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