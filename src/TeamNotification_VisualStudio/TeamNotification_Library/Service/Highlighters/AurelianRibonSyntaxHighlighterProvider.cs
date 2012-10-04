using System;
using AurelienRibon.Ui.SyntaxHighlightBox;
using TeamNotification_Library.Configuration;

namespace TeamNotification_Library.Service.Highlighters
{
    public class AurelianRibonSyntaxHighlighterProvider : IProvideSyntaxHighlighter<IHighlighter>
    {
        private IHighlighter CSharpHighlighter;
        private IHighlighter VBHighlighter;
        private IHighlighter JavaScriptHighlighter;
        private IHighlighter DefaultHighlighter;

        public AurelianRibonSyntaxHighlighterProvider()
        {
            CSharpHighlighter = HighlighterManager.Instance.Highlighters["cSharp"];
            VBHighlighter = HighlighterManager.Instance.Highlighters["vBNET"];
            JavaScriptHighlighter = HighlighterManager.Instance.Highlighters["javaScript"];
            DefaultHighlighter = HighlighterManager.Instance.Highlighters["default"];
        }

        public IHighlighter GetFor(int programmingLanguageIdentifier)
        {
            switch (programmingLanguageIdentifier)
            {
                case GlobalConstants.ProgrammingLanguages.CSharp:
                    return CSharpHighlighter;

                case GlobalConstants.ProgrammingLanguages.VisualBasic:
                    return VBHighlighter;

                case GlobalConstants.ProgrammingLanguages.JavaScript:
                    return JavaScriptHighlighter;

                default:
                    return DefaultHighlighter;
            }
        }
    }
}