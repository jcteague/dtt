using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;
using EnvDTE;

namespace TeamNotification_Library.Models.UI
{
    public class MessagesContainer
    {
        public Dictionary<string, TableRowGroup> MessagesList { get; set; }
        public ComboBox ComboRooms{ get; set; }
        public RichTextBox Container { get; set; }
        public RichTextBox InputBox { get; set; }
        public StatusBar StatusBar { get; set; }
        public Table MessagesTable { get; set; }
    }
}