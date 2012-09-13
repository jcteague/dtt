using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Models;
using TeamNotification_Library.Models.UI;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.Providers;

namespace TeamNotification_Library.Service.Controls
{
    public class MessagesEditor : IEditMessages
    {
        public RichTextBox inputMethod { get; set; }
        public Brush originalBackground { get; set; }
        public TableRowGroup currentRowGroup { get; set; }
        public Collection.Messages editingMessage { get; set; }
        public ChatMessageModel editingMessageModel { get; set; }

        public ComboBox comboRooms;
        private ISerializeJSON jsonSerializer;
        private IProvideUser userProvider;

        public MessagesEditor(ISerializeJSON jsonSerializer, IProvideUser userProvider)
        {
            this.jsonSerializer = jsonSerializer;
            this.userProvider = userProvider;
        }

        public void ConfigTableRowGroup(TableRowGroup row, Collection.Messages message, MessagesContainer messagesContainer)
        {
            inputMethod = messagesContainer.InputBox;
            comboRooms = messagesContainer.ComboRooms;
            row.Dispatcher.Invoke(new Action(() =>{
                if (row.Resources["originalMessage"] == null)
                {
                    row.Resources.Add("originalMessage", message);
                }
                else
                {
                    row.Resources["originalMessage"] = message;
                }
                row.MouseLeftButtonDown += EditMessage;
            }));
        }

        public void EditMessage(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (mouseButtonEventArgs.ClickCount != 2) return;

            var row = (TableRowGroup)sender;
            var tmpEditingMessage = row.Resources["originalMessage"].Cast<Collection.Messages>();
            var userId = Collection.getField(tmpEditingMessage.data, "user_id");
            var messageBody = Collection.getField(tmpEditingMessage.data, "body"); // row.Rows[0].Cells[1].GetText();
            var chatMessageBody = jsonSerializer.Deserialize<ChatMessageBody>(messageBody);

            if (userId != userProvider.GetUser().id.ToString() && !chatMessageBody.IsCode) return;
            if (currentRowGroup != null) ResetControls();
            editingMessage = tmpEditingMessage;

            var editingColor = new SolidColorBrush(Color.FromRgb(252, 249, 206));

            editingMessageModel = new ChatMessageModel
            {
                user_id = userId,
                username = Collection.getField(editingMessage.data, "user"),
                chatMessageBody = chatMessageBody
            };
            editingMessageModel.stamp = editingMessageModel.chatMessageBody.stamp;
            editingMessageModel.date = editingMessageModel.chatMessageBody.date;

            currentRowGroup = row;
            originalBackground = row.Background;
            row.Background = editingColor;
            inputMethod.Background = editingColor;
            comboRooms.IsEnabled = false;
            inputMethod.Document.Blocks.Clear();
            inputMethod.Document.Blocks.Add(new Paragraph(new Run(editingMessageModel.chatMessageBody.message)));
            inputMethod.Focus();
            inputMethod.PreviewKeyDown += CancelEditMessage;
            inputMethod.TextChanged += UpdateMessageData;
        }

        public void UpdateMessageData(object sender, EventArgs e)
        {
            var rtb = (RichTextBox)sender;
            var text = rtb.Document.GetDocumentText();
            if (text == "\r\n") ResetControls();
        }

        public void ResetControls()
        {
            inputMethod.Document.Blocks.Clear();
            if (editingMessage == null) return;
            currentRowGroup.Background = originalBackground;
            inputMethod.Background = originalBackground;
            inputMethod.PreviewKeyDown -= CancelEditMessage;
            inputMethod.TextChanged -= UpdateMessageData;
            currentRowGroup = null;
            editingMessage = null;
            editingMessageModel = null;
            comboRooms.IsEnabled = true;
        }

        public void CancelEditMessage(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != Key.Escape) return;
            ResetControls();
        }
    }
}
