using System.Windows.Documents;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Factories.UI
{
    public class SyntaxBlockUIContainerFactory : ICreateSyntaxBlockUIInstances
    {
        private readonly ICreateSyntaxHighlightBox syntaxHighlightBoxFactory;
        
        public SyntaxBlockUIContainerFactory(ICreateSyntaxHighlightBox syntaxHighlightBoxFactory)
        {
            this.syntaxHighlightBoxFactory = syntaxHighlightBoxFactory;
        }

        public BlockUIContainer Get(ChatMessageModel clipboardData)
        {
            return new BlockUIContainer(syntaxHighlightBoxFactory.Get(clipboardData.chatMessageBody.message, clipboardData.chatMessageBody.programminglanguage)) { Resources = clipboardData.AsResources() };
        }
    }
}