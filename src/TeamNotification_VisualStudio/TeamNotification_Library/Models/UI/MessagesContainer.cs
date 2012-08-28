using System.Windows.Controls;
using System.Windows.Documents;

namespace TeamNotification_Library.Models.UI
{
    public class MessagesContainer
    {
        public RichTextBox UsersList { get; set; }
        public RichTextBox MessagesList { get; set; }
        public RichTextBox DatesList { get; set; }

        public RichTextBox Container { get; set; }
        public Table MessagesTable { get; set; }
    }
}