using System;
using System.Windows;
using System.Windows.Controls;
using EnvDTE;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Models;
using TeamNotification_Library.Models.UI;
using TeamNotification_Library.Service.Chat;
using TeamNotification_Library.Service.Clipboard;
using TeamNotification_Library.Service.Factories.UI;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.LocalSystem;
using TeamNotification_Library.Service.Mappers;
using TeamNotification_Library.Service.Providers;
using TeamNotification_Library.Service.ToolWindow;

namespace TeamNotification_Library.Service.Controls
{
    public class ChatRoomsControlService : IServiceChatRoomsControl
    {
        private IProvideUser userProvider;
        private ISendHttpRequests httpClient;
        private IProvideConfiguration<ServerConfiguration> configuration;
        private IStoreClipboardData clipboardStorage;
        private ISerializeJSON jsonSerializer;
        private IMapEntities<MessageData, ChatMessageModel> messageDataToChatMessageModelMapper;
        private IMapEntities<Collection.Messages, ChatMessageModel> collectionMessagesToChatMessageModelMapper;
        private IGetToolWindowAction toolWindowActionGetter;
        private IHandleChatMessages chatMessagesService;
        
        readonly IStoreGlobalState applicationGlobalState;
        readonly ICreateSyntaxBlockUIInstances syntaxBlockUIContainerFactory;

        private readonly ISendChatMessages messageSender;
        private IHandleSystemClipboard systemClipboardHandler;

        public ChatRoomsControlService(IProvideUser userProvider, ISendHttpRequests httpClient, IProvideConfiguration<ServerConfiguration> configuration, IStoreClipboardData clipboardStorage, ISendChatMessages messageSender, IStoreGlobalState applicationGlobalState, IHandleSystemClipboard systemClipboardHandler, ICreateSyntaxBlockUIInstances syntaxBlockUIContainerFactory, ISerializeJSON jsonSerializer, IMapEntities<MessageData, ChatMessageModel> messageDataToChatMessageModelMapper, IHandleChatMessages chatMessagesService, IMapEntities<Collection.Messages, ChatMessageModel> collectionMessagesToChatMessageModelMapper, IGetToolWindowAction toolWindowActionGetter)
        {
            this.userProvider = userProvider;
            this.httpClient = httpClient;
            this.clipboardStorage = clipboardStorage;
            this.messageSender = messageSender;
            this.applicationGlobalState = applicationGlobalState;
            this.systemClipboardHandler = systemClipboardHandler;
            this.syntaxBlockUIContainerFactory = syntaxBlockUIContainerFactory;
            this.jsonSerializer = jsonSerializer;
            this.messageDataToChatMessageModelMapper = messageDataToChatMessageModelMapper;
            this.chatMessagesService = chatMessagesService;
            this.collectionMessagesToChatMessageModelMapper = collectionMessagesToChatMessageModelMapper;
            this.toolWindowActionGetter = toolWindowActionGetter;
            this.configuration = configuration;
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
                var activeProjects = dte.ActiveDocument.ProjectItem.ContainingProject;
                var message = systemClipboardHandler.GetText(true);
                var clipboard = new CodeClipboardData
                {
					project = activeProjects.UniqueName,
                    solution = dte.Solution.FullName,
                    document = activeDocument.FullName,
                    message = message,
                    line = selection.CurrentLine,
                    column = selection.CurrentColumn,
                    programmingLanguage = activeDocument.GetProgrammingLanguage()
                };

                clipboardStorage.Store(clipboard);
            }
            else
            {
                var clipboard = new PlainClipboardData {message = systemClipboardHandler.GetText(true)};
                clipboardStorage.Store(clipboard);
            }
        }

        public void HandlePaste(RichTextBox textBox, DataObjectPastingEventArgs dataObjectPastingEventArgs)
        {
            if (!clipboardStorage.IsCode) return;
            var block = syntaxBlockUIContainerFactory.Get(clipboardStorage.Get<CodeClipboardData>());
            textBox.Document.Blocks.Add(block);
            textBox.CaretPosition = textBox.Document.ContentEnd;
            dataObjectPastingEventArgs.CancelCommand();
        }

        public void HandleDock(ChatUIElements chatUIElements)
        {
            toolWindowActionGetter.Get().ExecuteOn(chatUIElements);
        }

        public void SendMessage(RichTextBox textBox, string roomId)
        {
            messageSender.SendMessages(textBox.Document.Blocks, roomId);
            textBox.Document.Blocks.Clear();
        }

        public void ResetContainer(ChatUIElements messagesContainer)
        {
            messagesContainer.MessagesTable.RowGroups.Clear();
        }

        public void AddMessages(ChatUIElements messagesContainer, ScrollViewer scrollviewer, string currentRoomId)
        {
            chatMessagesService.ResetUser();
            var collection = GetMessagesCollection(currentRoomId);
            foreach (var message in collection.messages)
            {
                chatMessagesService.AppendMessage(messagesContainer, scrollviewer, collectionMessagesToChatMessageModelMapper.MapFrom(message));
            }
        }
        
        public void AddReceivedMessage(ChatUIElements messagesContainer, ScrollViewer scrollviewer, string messageData)
        {
            var m = jsonSerializer.Deserialize<MessageData>(messageData);
            chatMessagesService.AppendMessage(messagesContainer, scrollviewer, messageDataToChatMessageModelMapper.MapFrom(m));
        }
    }
}