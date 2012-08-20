using System.Collections.Generic;
using System.Windows.Media;
using AurelienRibon.Ui.SyntaxHighlightBox;
using System.Linq;
using TeamNotification_Library.Service.Highlighters.Rules;

namespace TeamNotification_Library.Service.Highlighters
{
    public class CSharpSyntaxHighligher : IHighlighter
    {
        private IHighlightWords wordsHighlighter;
        private IHighlightNumbers numbersHighlighter;

        public CSharpSyntaxHighligher(IHighlightWords wordsHighlighter, IHighlightNumbers numbersHighlighter)
        {
            this.wordsHighlighter = wordsHighlighter;
            this.numbersHighlighter = numbersHighlighter;
        }

        public int Highlight(FormattedText text, int previousBlockCode)
        {
            wordsHighlighter.Format(text, previousBlockCode);

            numbersHighlighter.Format(text, previousBlockCode);

            return 0;
        }
    }
}