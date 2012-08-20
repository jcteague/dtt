using System.Windows.Media;

namespace TeamNotification_Library.Service.Highlighters.Rules
{
    public interface IFormatSyntaxAccordingToRule
    {
        int Format(FormattedText text, int previousBlockCode);
    }
}