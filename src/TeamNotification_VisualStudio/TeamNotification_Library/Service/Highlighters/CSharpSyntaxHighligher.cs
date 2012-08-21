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
        private IHighlightStrings stringsHighlighter;
        private IHighlightLineComments lineCommentsHighlighter;
        private IHighlightLongComments longCommentsHighlighter;

        public CSharpSyntaxHighligher(IHighlightWords wordsHighlighter, IHighlightNumbers numbersHighlighter, IHighlightStrings stringsHighlighter, IHighlightLineComments lineCommentsHighlighter, IHighlightLongComments longCommentsHighlighter)
        {
            this.wordsHighlighter = wordsHighlighter;
            this.numbersHighlighter = numbersHighlighter;
            this.stringsHighlighter = stringsHighlighter;
            this.lineCommentsHighlighter = lineCommentsHighlighter;
            this.longCommentsHighlighter = longCommentsHighlighter;
        }

        public int Highlight(FormattedText text, int previousBlockCode)
        {
            wordsHighlighter.Format(text, previousBlockCode);

            numbersHighlighter.Format(text, previousBlockCode);

            stringsHighlighter.Format(text, previousBlockCode);

            lineCommentsHighlighter.Format(text, previousBlockCode);

            var code = longCommentsHighlighter.Format(text, previousBlockCode);

            return code;
        }
    }
}