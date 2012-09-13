using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using TeamNotification_Library.Models;
using TeamNotification_Library.Models.UI;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace TeamNotification_Library.Service.Chat.Formatters
{
    public interface IEditMessages
    {
        RichTextBox inputMethod { get; set; }
        Brush originalBackground { get; set; }
        TableRowGroup currentRowGroup { get; set; }
        Collection.Messages editingMessage { get; set; }
        ChatMessageModel editingMessageModel { get; set; }

        void ConfigTableRowGroup(TableRowGroup row, Collection.Messages message, MessagesContainer messagesContainer);
        void ResetControls();
        void EditMessage(object sender, MouseButtonEventArgs mouseButtonEventArgs);
        void UpdateMessageData(object sender, EventArgs e);
        void CancelEditMessage(object sender, KeyEventArgs e);
    }
}
