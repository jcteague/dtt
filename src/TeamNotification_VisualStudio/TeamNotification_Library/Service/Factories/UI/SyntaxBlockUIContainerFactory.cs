using System.Windows.Documents;
using AurelienRibon.Ui.SyntaxHighlightBox;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Factories.UI
{
    public class SyntaxBlockUIContainerFactory : ICreateSyntaxBlockUIInstances
    {
        public BlockUIContainer Get(CodeClipboardData clipboardData)
        {
            return new BlockUIContainer(
                new SyntaxHighlightBox
                {
                    Text = clipboardData.message,
                    CurrentHighlighter = HighlighterManager.Instance.Highlighters["cSharp"]
                }) { Resources = clipboardData.AsResources() };
        }
    }
}