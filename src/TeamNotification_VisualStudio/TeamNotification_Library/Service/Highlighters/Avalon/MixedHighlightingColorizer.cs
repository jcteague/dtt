using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Rendering;

namespace TeamNotification_Library.Service.Highlighters.Avalon
{
    public class MixedHighlightingColorizer : HighlightingColorizer
    {
        public MixedHighlightingColorizer(HighlightingRuleSet ruleSet) : base(ruleSet)
        {


        }

        protected override void Colorize(ITextRunConstructionContext context)
        {
            if (LineIsCode(context))
                base.Colorize(context);
        }

        private bool LineIsCode(ITextRunConstructionContext context)
        {
            return true;
        }
    }
}