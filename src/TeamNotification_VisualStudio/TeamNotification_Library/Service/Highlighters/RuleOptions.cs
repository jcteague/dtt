using System.Windows;
using System.Windows.Media;

namespace TeamNotification_Library.Service.Highlighters
{
    public class RuleOptions
    {
        public bool IgnoreCase { get; private set; }
        public Brush Foreground { get; private set; }
        public FontWeight FontWeight { get; private set; }
        public FontStyle FontStyle { get; private set; }

        public RuleOptions(string foreground, string fontWeight, string fontStyle)
        {
            string foregroundStr = foreground.Trim();
            string fontWeightStr = fontWeight.Trim();
            string fontStyleStr = fontStyle.Trim();

            Foreground = (Brush)new BrushConverter().ConvertFrom(foregroundStr);
            FontWeight = (FontWeight)new FontWeightConverter().ConvertFrom(fontWeightStr);
            FontStyle = (FontStyle)new FontStyleConverter().ConvertFrom(fontStyleStr);
        }
    }
}