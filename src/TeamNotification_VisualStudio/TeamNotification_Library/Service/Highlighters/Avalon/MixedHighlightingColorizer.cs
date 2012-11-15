using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Rendering;
using TeamNotification_Library.Extensions;

namespace TeamNotification_Library.Service.Highlighters.Avalon
{
    public class MixedHighlightingColorizer : HighlightingColorizer
    {
        private readonly TextEditor textEditor;

        public MixedHighlightingColorizer(HighlightingRuleSet ruleSet, TextEditor textEditor) : base(ruleSet)
        {
            this.textEditor = textEditor;
        }

        protected override void Colorize(ITextRunConstructionContext context)
        {
            var content = (SortedList<int, object>) textEditor.Resources["content"];
            if (LineIsCode(context, content))
                base.Colorize(context);
        }

        private bool LineIsCode(ITextRunConstructionContext context, SortedList<int, object> content)
        {
            if (content.IsNull())
                return false;

            var codeLines = content.Where(x => !(x.Value is string)).Select(x => x.Key);
            var line = context.VisualLine.LastDocumentLine.LineNumber;
            if (codeLines.Contains(line))
                return true;
            
            return false;
        }
    }
}