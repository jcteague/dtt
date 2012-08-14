using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using EnvDTE;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Controls
{
    public interface IServiceChatRoomsControl
    {
        Collection GetCollection();
        Collection GetMessagesCollection(string roomId);
        void UpdateClipboard(object source, DTE dte);
        void HandlePaste(RichTextBox textBox, DataObjectPastingEventArgs dataObjectPastingEventArgs);
        void SendMessage(RichTextBox textBox, string roomId);
        bool HasClipboardData { get; set; }
        void ClearRichTextBox(RichTextBox textBox);
    }
}