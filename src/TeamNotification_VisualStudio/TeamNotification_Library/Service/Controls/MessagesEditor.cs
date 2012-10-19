using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using ICSharpCode.AvalonEdit;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Models;
using TeamNotification_Library.Models.UI;
using TeamNotification_Library.Service.Factories.UI.Highlighters;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.LocalSystem;
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
        public IStoreGlobalState applicationGlobalState;
        public ComboBox comboRooms;
        private Brush editingColor;
        private ISerializeJSON jsonSerializer;
        private IShowCode codeEditor;
        private ICreateSyntaxHighlightBox<TextEditor> textEditorCreator;
        public MessagesEditor(ISerializeJSON jsonSerializer, IStoreGlobalState applicationGlobalState, ICreateSyntaxHighlightBox<TextEditor> textEditorCreator)
        {
            editingColor =  new SolidColorBrush(Color.FromRgb(252, 249, 206));
            this.jsonSerializer = jsonSerializer;
            this.applicationGlobalState = applicationGlobalState;
            this.textEditorCreator = textEditorCreator;
        }

        public void ConfigTableRowGroup(TableRowGroup row, Collection.Messages message, ChatUIElements messagesContainer)
        {
            codeEditor = messagesContainer.codeEditor;
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
                inputMethod.Document.Blocks.Clear();
                var editedCode = codeEditor.Show(editingMessageModel.chatMessageBody.message,
                                                 editingMessageModel.chatMessageBody.programminglanguage);
                var editor = textEditorCreator.Get(editedCode, editingMessageModel.chatMessageBody.programminglanguage);
                editingMessageModel.chatMessageBody.message = editedCode;
                var blockUIContainer = new BlockUIContainer(editor) {Resources = editingMessageModel.AsResources()};
                inputMethod.Document.Blocks.Add(blockUIContainer);
            }else{
                SetControls(row);
                applicationGlobalState.IsEditingCode = true;
            }
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
            if (!editingMessageModel.chatMessageBody.IsCode){
                currentRowGroup.Background = originalBackground;
                inputMethod.Background = originalBackground;
            
                inputMethod.PreviewKeyDown -= CancelEditMessage;
                inputMethod.TextChanged -= OnInputMethodTextChanged;
                inputMethod.LostFocus -= inputMethod_LostFocus;
            }
            currentRowGroup = null;
            editingMessage = null;
            editingMessageModel = null;
            comboRooms.IsEnabled = true;
            applicationGlobalState.IsEditingCode = false;
        }

        public void CancelEditMessage(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != Key.Escape) return;
            ResetControls();
            e.Handled = true;
        }
    }
}
