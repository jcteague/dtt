using System.Windows.Documents;
using ICSharpCode.AvalonEdit;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Factories.UI.Highlighters
{
    public class SyntaxBlockUIContainerFactory : ICreateSyntaxBlockUIInstances
    {
        private readonly ICreateSyntaxHighlightBox<TextEditor> syntaxHighlightBoxFactory;

        public SyntaxBlockUIContainerFactory(ICreateSyntaxHighlightBox<TextEditor> syntaxHighlightBoxFactory)
        {
            this.syntaxHighlightBoxFactory = syntaxHighlightBoxFactory;
        }

        public BlockUIContainer Get(ChatMessageModel clipboardData)
        {
            return new BlockUIContainer(syntaxHighlightBoxFactory.Get(clipboardData.chatMessageBody.message, clipboardData.chatMessageBody.programminglanguage)) { Resources = clipboardData.AsResources() };
        }
    }
}