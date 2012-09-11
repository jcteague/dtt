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

        public SyntaxHighlighterProvider()
        {
            CSharpHighlighter = HighlighterManager.Instance.Highlighters["cSharp"];
            VBHighlighter = HighlighterManager.Instance.Highlighters["vBNET"];
            JavaScriptHighlighter = HighlighterManager.Instance.Highlighters["javaScript"];
        }

        public IHighlighter GetFor(int programminglanguageIdentifier)
        {
            switch (programminglanguageIdentifier)
            {
                case Globals.programminglanguages.CSharp:
                    return CSharpHighlighter;

                case Globals.programminglanguages.VisualBasic:
                    return VBHighlighter;

                case Globals.programminglanguages.JavaScript:
                    return JavaScriptHighlighter;

                default:
                    throw new ArgumentException("There is no implementation for the provided programming language");
            }
        }
    }
}