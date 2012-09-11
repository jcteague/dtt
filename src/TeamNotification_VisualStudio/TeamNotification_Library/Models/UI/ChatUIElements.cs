using System.Windows.Controls;
using System.Windows.Documents;
using EnvDTE;

namespace TeamNotification_Library.Models.UI
{
    public class ChatUIElements
    {
        public StatusBar StatusBar { get; set; }
        public RichTextBox Container { get; set; }
        public Table MessagesTable { get; set; }
        public RichTextBox MessageInput { get; set; }
        public Border MessageTextBoxBorder { get; set; }
        public Border MessageContainerBorder { get; set; }
    }
}