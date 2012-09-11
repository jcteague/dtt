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
        public Grid MessageTextBoxGrid { get; set; }
        public Border MessageContainerBorder { get; set; }

        public RowDefinition MessageRow1 { get; set; }
        public RowDefinition MessageRow2 { get; set; }

        public ColumnDefinition MessageColumn1 { get; set; }
        public ColumnDefinition MessageColumn2 { get; set; }

        public Button SendMessageButton { get; set; }

        public RowDefinition OuterGridRow3 { get; set; }
    }
}