using System.Windows.Documents;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Factories.UI.Highlighters
{
    public interface ICreateSyntaxBlockUIInstances
    {
        BlockUIContainer Get(ChatMessageModel clipboardData);
    }
}