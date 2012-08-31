using System.Windows.Controls;
using System.Windows.Documents;
using EnvDTE;

namespace TeamNotification_Library.Models.UI
{
    public class MessagesContainer
    {
        public StatusBar StatusBar { get; set; }
        public RichTextBox Container { get; set; }
        public Table MessagesTable { get; set; }
    }
}