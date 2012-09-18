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
        private Brush editingColor;
        private ISerializeJSON jsonSerializer;

        public MessagesEditor(ISerializeJSON jsonSerializer)
        {
            editingColor =  new SolidColorBrush(Color.FromRgb(252, 249, 206));
            this.jsonSerializer = jsonSerializer;
        }

        public void ConfigTableRowGroup(TableRowGroup row, Collection.Messages message, ChatUIElements messagesContainer)
        {
            var currentStamp = Collection.getField(message.data, "stamp");
            var charMessageBody = jsonSerializer.Deserialize<ChatMessageBody>(Collection.getField(message.data,"body"));
            var containsKey = false;
            if (!charMessageBody.IsCode){
                if (!messagesContainer.LastStamp.IsNullOrEmpty() && messagesContainer.MessagesList.ContainsKey(messagesContainer.LastStamp))
                {
                    messagesContainer.MessagesList[messagesContainer.LastStamp].MouseLeftButtonDown -= EditMessage;
                    messagesContainer.MessagesList.Remove(messagesContainer.LastStamp);
                }
                containsKey = messagesContainer.MessagesList.ContainsKey(currentStamp);
                if (!containsKey) messagesContainer.MessagesList.Add(currentStamp,row);

                messagesContainer.LastStamp = currentStamp;
            }
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
                if (!containsKey) row.MouseLeftButtonDown += EditMessage;
            }));
        }

        public void EditMessage(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (mouseButtonEventArgs.ClickCount != 2) return;

            var row = (TableRowGroup)sender;
            if (currentRowGroup != null) ResetControls();

            editingMessage = row.Resources["originalMessage"].Cast<Collection.Messages>();
            
            if (editingMessage == null) return;

            var userId = Collection.getField(editingMessage.data, "user_id");


            editingMessageModel = new ChatMessageModel
            {
                user_id = userId,
                username = Collection.getField(editingMessage.data, "user"),
                body = Collection.getField(editingMessage.data, "body")
            };

            var chatMessageBody = editingMessageModel.chatMessageBody;

            if (chatMessageBody.IsCode)
            {
                chatMessageBody.stamp = "";
                editingMessageModel.chatMessageBody = chatMessageBody;
            }
            SetControls(row);
        }

        private void SetControls(TableRowGroup row)
        {
            currentRowGroup = row;
            originalBackground = row.Background;
            row.Background = editingColor;
            inputMethod.Background = editingColor;
            comboRooms.IsEnabled = false;
            inputMethod.Document.Blocks.Clear();
            inputMethod.Document.Blocks.Add(new Paragraph(new Run(editingMessageModel.chatMessageBody.message)));
            inputMethod.Focus();
            inputMethod.PreviewKeyDown += CancelEditMessage;
            inputMethod.TextChanged += OnInputMethodTextChanged;
            inputMethod.LostFocus += inputMethod_LostFocus;
        }

        void inputMethod_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if((Keyboard.GetKeyStates(Key.Escape) & KeyStates.Down) > 0)
            {
                ResetControls();
                inputMethod.Focus();
            }
        }

        public void OnInputMethodTextChanged(object sender, EventArgs e)
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
            inputMethod.TextChanged -= OnInputMethodTextChanged;
            inputMethod.LostFocus -= inputMethod_LostFocus;

            currentRowGroup = null;
            editingMessage = null;
            editingMessageModel = null;
            comboRooms.IsEnabled = true;
        }

        public void CancelEditMessage(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != Key.Escape) return;
            ResetControls();
            e.Handled = true;
        }
    }
}
