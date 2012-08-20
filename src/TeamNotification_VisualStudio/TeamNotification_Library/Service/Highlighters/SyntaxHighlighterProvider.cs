using System;
using AurelienRibon.Ui.SyntaxHighlightBox;
using TeamNotification_Library.Configuration;

namespace TeamNotification_Library.Service.Highlighters
{
    public class SyntaxHighlighterProvider : IProvideSyntaxHighlighter
    {
        private IHighlighter CSharpHighlighter;
        private IHighlighter VBHighlighter;
        private IHighlighter JavaScriptHighlighter;

        public SyntaxHighlighterProvider(IHighlighter cSharpHighlighter)
        {
            CSharpHighlighter = cSharpHighlighter;
//            CSharpHighlighter = HighlighterManager.Instance.Highlighters["cSharp"];
//            CSharpHighlighter = new CSharpSyntaxHighligher();
//            VBHighlighter = HighlighterManager.Instance.Highlighters["visualBasic"];
//            JavaScriptHighlighter = HighlighterManager.Instance.Highlighters["javaScript"];
        }

        public IHighlighter GetFor(int programmingLanguageIdentifier)
        {
            switch (programmingLanguageIdentifier)
            {
                case Globals.ProgrammingLanguages.CSharp:
                    return CSharpHighlighter;

                case Globals.ProgrammingLanguages.VisualBasic:
                    return VBHighlighter;

                case Globals.ProgrammingLanguages.JavaScript:
                    return JavaScriptHighlighter;

                default:
                    throw new ArgumentException("There is no implementation for the provided programming language");
            }
        }
    }
}