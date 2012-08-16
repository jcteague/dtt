using System.Windows.Documents;
using AurelienRibon.Ui.SyntaxHighlightBox;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Factories.UI
{
    public class SyntaxBlockUIContainerFactory : ICreateSyntaxBlockUIInstances
    {
        private IHighlighter highlighter;

        public SyntaxBlockUIContainerFactory()
        {
            highlighter = HighlighterManager.Instance.Highlighters["cSharp"];
        }

        public BlockUIContainer Get(CodeClipboardData clipboardData)
        {
            return new BlockUIContainer(
                new SyntaxHighlightBox
                {
                    Text = clipboardData.message,
                    CurrentHighlighter = highlighter
                }) { Resources = clipboardData.AsResources() };
        }
    }
}