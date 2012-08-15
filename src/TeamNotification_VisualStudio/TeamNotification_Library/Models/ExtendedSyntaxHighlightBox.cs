using AurelienRibon.Ui.SyntaxHighlightBox;

namespace TeamNotification_Library.Models
{
    public class ExtendedSyntaxHighlightBox : SyntaxHighlightBox
    {
        public ExtendedSyntaxHighlightBox() : base()
        {
        }

        public string Solution { get; set; }
        public string File { get; set; }
        public int Line { get; set; }
    }
}