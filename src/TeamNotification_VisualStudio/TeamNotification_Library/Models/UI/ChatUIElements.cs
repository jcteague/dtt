using System.Collections.Generic;
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
        public RowDefinition MessageGridRowDefinition1 { get; set; }
        public RowDefinition MessageGridRowDefinition2 { get; set; }
        public ColumnDefinition MessageGridColumnDefinition1 { get; set; }
        public ColumnDefinition MessageGridColumnDefinition2 { get; set; }
        public Button SendMessageButton { get; set; }
        public RowDefinition OuterGridRowDefinition3 { get; set; }
        public GridSplitter MessageTextBoxGridSplitter { get; set; }
        public string LastStamp { get; set; }
        public Dictionary<string, TableRowGroup> MessagesList { get; set; }
        public ComboBox ComboRooms { get; set; }
        public RichTextBox InputBox { get; set; }
    }
}