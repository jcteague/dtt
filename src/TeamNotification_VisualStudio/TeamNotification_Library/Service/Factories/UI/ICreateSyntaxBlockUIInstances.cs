using System.Windows.Documents;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Factories.UI
{
    public interface ICreateSyntaxBlockUIInstances
    {
        BlockUIContainer Get(CodeClipboardData clipboardData, int programmingLanguage);
    }
}