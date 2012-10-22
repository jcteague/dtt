using ICSharpCode.AvalonEdit.Highlighting;
using TeamNotification_Library.Configuration;

namespace TeamNotification_Library.Service.Highlighters
{
    public class AvalonSyntaxHighlighterProvider : IProvideSyntaxHighlighter<IHighlightingDefinition>
    {
        public IHighlightingDefinition GetFor(int programmingLanguageIdentifier)
        {
            switch (programmingLanguageIdentifier)
            {
                case GlobalConstants.ProgrammingLanguages.CSharp:
                    return HighlightingManager.Instance.GetDefinition("C#");

                case GlobalConstants.ProgrammingLanguages.VisualBasic:
                    return HighlightingManager.Instance.GetDefinition("VBNET");

                case GlobalConstants.ProgrammingLanguages.JavaScript:
                    return HighlightingManager.Instance.GetDefinition("JavaScript");

                default:
                    return null;
            }
        }
    }
}