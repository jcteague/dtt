using System.Collections.Generic;
using System.Windows.Media;
using AurelienRibon.Ui.SyntaxHighlightBox;
using System.Linq;

namespace TeamNotification_Library.Service.Highlighters
{
    public class CSharpSyntaxHighligher : IHighlighter
    {
        public int Highlight(FormattedText text, int previousBlockCode)
        {
            if (IsHighlightableWord(text.Text))
            {
                var brush = new SolidColorBrush() { Color = Colors.Blue };
                text.SetForegroundBrush(brush);
            }

            return 0;
        }

        private bool IsHighlightableWord(string text)
        {
            return HighlightableWords.Contains(text);
        }

        private List<string> HighlightableWords = new List<string>
                                                      {
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