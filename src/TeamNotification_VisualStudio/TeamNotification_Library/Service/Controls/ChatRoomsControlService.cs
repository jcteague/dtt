using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using EnvDTE;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Models;
using TeamNotification_Library.Models.UI;
using TeamNotification_Library.Service.Chat;
using TeamNotification_Library.Service.Chat.Formatters;
using TeamNotification_Library.Service.Clipboard;
using TeamNotification_Library.Service.Factories.UI;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.LocalSystem;
using TeamNotification_Library.Service.Mappers;
using TeamNotification_Library.Service.Providers;

namespace TeamNotification_Library.Service.Controls
{
    public class ChatRoomsControlService : IServiceChatRoomsControl
    {
        private IProvideUser userProvider;
        private ISendHttpRequests httpClient;
        private IProvideConfiguration<ServerConfiguration> configuration;
        private IStoreClipboardData clipboardStorage;
        private ISerializeJSON jsonSerializer;
        //private IMapEntities<MessageData, ChatMessageModel> messageDataToChatMessageModelMapper;
        private IMapEntities<Collection.Messages, ChatMessageModel> collectionMessagesToChatMessageModelMapper;
        private IHandleChatMessages chatMessagesService;
        private Dictionary<string, TableRow> MessagesRowsList { get; set; }
        
        readonly IStoreGlobalState applicationGlobalState;
        readonly ICreateSyntaxBlockUIInstances syntaxBlockUIContainerFactory;

        private readonly ISendChatMessages messageSender;
        private IHandleSystemClipboard systemClipboardHandler;
        private IEditMessages messagesEditor;

        public ChatRoomsControlService(IProvideUser userProvider, ISendHttpRequests httpClient, IProvideConfiguration<ServerConfiguration> configuration, IStoreClipboardData clipboardStorage, ISendChatMessages messageSender, IStoreGlobalState applicationGlobalState, IHandleSystemClipboard systemClipboardHandler, ICreateSyntaxBlockUIInstances syntaxBlockUIContainerFactory, ISerializeJSON jsonSerializer, IHandleChatMessages chatMessagesService, IMapEntities<Collection.Messages, ChatMessageModel> collectionMessagesToChatMessageModelMapper, IEditMessages messagesEditor)
        {
            this.userProvider = userProvider;
            this.httpClient = httpClient;
            this.clipboardStorage = clipboardStorage;
            this.messageSender = messageSender;
            this.applicationGlobalState = applicationGlobalState;
            this.systemClipboardHandler = systemClipboardHandler;
            this.syntaxBlockUIContainerFactory = syntaxBlockUIContainerFactory;
            this.jsonSerializer = jsonSerializer;
            this.messagesEditor = messagesEditor;
            //this.messageDataToChatMessageModelMapper = messageDataToChatMessageModelMapper;
            this.chatMessagesService = chatMessagesService;
            this.collectionMessagesToChatMessageModelMapper = collectionMessagesToChatMessageModelMapper;
            this.configuration = configuration;
            MessagesRowsList = new Dictionary<string, TableRow>();
        }

        public Collection GetMessagesCollection(string roomId)
        {
            var uri = configuration.Get().Uri + "room/" + roomId + "/messages";
            var c = httpClient.Get<Collection>(uri).Result;
            return c;
        }

        public Collection GetCollection()
        {
            var user = userProvider.GetUser();
            var uri = configuration.Get().Uri +"user/"+ user.id;
            var c = httpClient.Get<Collection>(uri).Result;
            return c;
        }

        public void UpdateClipboard(object source, DTE dte)
        {
            if (applicationGlobalState.Active && dte.ActiveWindow.Document.IsNotNull())
            {
                var activeDocument = dte.ActiveDocument;
                var txt = activeDocument.Object() as TextDocument;
                if (txt.IsNull()) return;
                var selection = txt.Selection;
				var activeProjects = dte.ActiveSolutionProjects[0];
                var message = systemClipboardHandler.GetText(true);
                var clipboard = new ChatMessageModel
                {
                    chatMessageBody = new ChatMessageBody
                    {
					    project = activeProjects.UniqueName,
                        solution = dte.Solution.FullName,
                        document = activeDocument.FullName,
                        message = message,
                        line = selection.CurrentLine,
                        column = selection.CurrentColumn,
                        programminglanguage = activeDocument.Getprogramminglanguage()
                    }
                };

                clipboardStorage.Store(clipboard);
            }
            else
            {
                var clipboard = new ChatMessageModel{ chatMessageBody = new ChatMessageBody {message = systemClipboardHandler.GetText(true)}};
                clipboardStorage.Store(clipboard);
            }
        }

        public void HandlePaste(RichTextBox textBox, DataObjectPastingEventArgs dataObjectPastingEventArgs)
        {
            var chatMessageModel = clipboardStorage.Get<ChatMessageModel>();

            if (chatMessageModel.chatMessageBody.IsCode)
            {
                var block = syntaxBlockUIContainerFactory.Get(chatMessageModel);
                textBox.Document.Blocks.Add(block);
                textBox.CaretPosition = textBox.Document.ContentEnd;
                dataObjectPastingEventArgs.CancelCommand();
            }
        }

        public void SendMessage(RichTextBox textBox, string roomId)
        {
            messagesEditor.inputMethod = textBox;
            if (messagesEditor.editingMessage != null)
            {
                var text = messagesEditor.inputMethod.Document.GetDocumentText();
                messagesEditor.editingMessageModel.chatMessageBody.message = text.Substring(0, text.Length - 2);
                messageSender.SendMessage(messagesEditor.editingMessageModel.chatMessageBody, roomId);
            }
            else
            {
                messageSender.SendMessages(messagesEditor.inputMethod.Document.Blocks, roomId);
            }
            messagesEditor.ResetControls();
        }

        public void ResetContainer(MessagesContainer messagesContainer)
        {
            messagesContainer.MessagesTable.RowGroups.Clear();
        }

        public void AddMessages(MessagesContainer messagesContainer, ScrollViewer scrollviewer, string currentRoomId)
        {
            chatMessagesService.ResetUser();
            var collection = GetMessagesCollection(currentRoomId);
            foreach (var message in collection.messages)
            {
                chatMessagesService.AppendMessage(messagesContainer, scrollviewer, collectionMessagesToChatMessageModelMapper.MapFrom(message));
                var idx = messagesContainer.MessagesTable.RowGroups.Count - 1;
                messagesEditor.ConfigTableRowGroup(messagesContainer.MessagesTable.RowGroups[idx], message, messagesContainer);
            }
        }

        public void AddReceivedMessage(MessagesContainer messagesContainer, ScrollViewer scrollviewer, string messageData)
        {
            var chatMessageModel = jsonSerializer.Deserialize<ChatMessageModel>(messageData);
            chatMessageModel.chatMessageBody = jsonSerializer.Deserialize<ChatMessageBody>(chatMessageModel.body);

            chatMessagesService.AppendMessage(messagesContainer, scrollviewer, chatMessageModel);
            //var idx = messagesContainer.MessagesTable.RowGroups.Count - 1;
            var collectionMessage = ChatMessageModelToCollectionMessage(chatMessageModel);
            var rowGroup = messagesContainer.MessagesList[chatMessageModel.stamp];
            messagesEditor.ConfigTableRowGroup(rowGroup, collectionMessage, messagesContainer);
        }

        private Collection.Messages ChatMessageModelToCollectionMessage(ChatMessageModel chatMessageModel)
        {
            return new Collection.Messages
            {
                data = new List<CollectionData>
                {
                    new CollectionData {name = "body", value = jsonSerializer.Serialize(chatMessageModel.chatMessageBody)},
                    new CollectionData {name = "user_id", value = chatMessageModel.user_id},
                    new CollectionData {name = "user", value = chatMessageModel.username},
                    new CollectionData {name = "stamp", value = chatMessageModel.chatMessageBody.stamp},
                    new CollectionData {name = "datetime", value = chatMessageModel.date},
                }
            };
        }

        //private RichTextBox inputMethod;
        //private Brush originalBackground;
        //private TableRowGroup currentRowGroup;
        //private Collection.Messages editingMessage;
        //private ChatMessageModel editingMessageModel;
        //private ComboBox comboRooms;
        //private void ConfigTableRowGroup(TableRowGroup row, Collection.Messages message, MessagesContainer messagesContainer)
        //{
        //    inputMethod = messagesContainer.InputBox;
        //    comboRooms = messagesContainer.ComboRooms;
        //    row.Dispatcher.Invoke(new Action(() =>{
        //        if (row.Resources["originalMessage"] == null)
        //            row.Resources.Add("originalMessage", message);
        //        else             
        //            row.Resources["originalMessage"] = message;
        //        row.MouseLeftButtonDown += EditMessage;
        //    }));
        //}

        //private void EditMessage (object sender, MouseButtonEventArgs mouseButtonEventArgs)
        //{
        //    if (mouseButtonEventArgs.ClickCount != 2) return;

        //    var row = (TableRowGroup) sender;
        //    var tmpEditingMessage = row.Resources["originalMessage"].Cast<Collection.Messages>();
        //    var userId = Collection.getField(editingMessage.data, "user_id");
        //    var messageBody = Collection.getField(editingMessage.data, "body"); // row.Rows[0].Cells[1].GetText();
        //    var chatMessageBody = jsonSerializer.Deserialize<ChatMessageBody>(messageBody);
           
        //    if (userId != userProvider.GetUser().id.ToString() && !chatMessageBody.IsCode) return;
        //    if (currentRowGroup != null) ResetControls();
        //    editingMessage = tmpEditingMessage;

        //    var editingColor = new SolidColorBrush(Color.FromRgb(252, 249, 206));
            
        //    editingMessageModel = new ChatMessageModel
        //    {
        //        user_id = userId,
        //        username = Collection.getField(editingMessage.data, "user"),
        //        chatMessageBody = chatMessageBody
        //    };
        //    editingMessageModel.stamp = editingMessageModel.chatMessageBody.stamp;
        //    editingMessageModel.date = editingMessageModel.chatMessageBody.date;

        //    currentRowGroup = row;
        //    originalBackground = row.Background; 
        //    row.Background = editingColor;
        //    inputMethod.Background = editingColor;
        //    comboRooms.IsEnabled = false;
        //    inputMethod.Document.Blocks.Clear();
        //    inputMethod.Document.Blocks.Add(new Paragraph(new Run(editingMessageModel.chatMessageBody.message)));
        //    inputMethod.Focus();
        //    inputMethod.PreviewKeyDown += CancelEditMessage;
        //    inputMethod.TextChanged += UpdateMessageData;
        //}

        //private void UpdateMessageData(object sender, EventArgs e)
        //{
        //    var rtb = (RichTextBox) sender;
        //    var text = rtb.Document.GetDocumentText();
        //    if (text == "\r\n") ResetControls();
        //}

        //private void ResetControls()
        //{
        //    inputMethod.Document.Blocks.Clear();
        //    if (editingMessage == null) return;
        //    currentRowGroup.Background = originalBackground;
        //    inputMethod.Background = originalBackground;
        //    inputMethod.PreviewKeyDown -= CancelEditMessage;
        //    inputMethod.TextChanged -= UpdateMessageData;
        //    currentRowGroup = null;
        //    editingMessage = null;
        //    editingMessageModel = null;
        //    comboRooms.IsEnabled = true;
        //}

        //private void CancelEditMessage(object sender, KeyEventArgs e)
        //{
        //    if (e.Key != Key.Escape) return;
        //    ResetControls();
        //}
    }
}