using System.Windows;
using System.Windows.Controls;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Models;
using TeamNotification_Library.Models.UI;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.Async.Models;
using TeamNotification_Library.Service.Chat;
using TeamNotification_Library.Service.Clipboard;
using TeamNotification_Library.Service.Factories.UI;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.Mappers;
using TeamNotification_Library.Service.Providers;
using TeamNotification_Library.Service.ToolWindow;

namespace TeamNotification_Library.Service.Controls
{
    // TODO: This class is doing too much things. Must separate concerns
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
        private readonly IStoreDataLocally localStorageService;
        private readonly IHandleUserAccountEvents userAccountEvents;
        
        readonly ICreateSyntaxBlockUIInstances syntaxBlockUIContainerFactory;
        private readonly ISendChatMessages messageSender;

        public ChatRoomsControlService(IProvideUser userProvider, ISendHttpRequests httpClient, IProvideConfiguration<ServerConfiguration> configuration, IStoreClipboardData clipboardStorage, ISendChatMessages messageSender, ICreateSyntaxBlockUIInstances syntaxBlockUIContainerFactory, ISerializeJSON jsonSerializer, IMapEntities<MessageData, ChatMessageModel> messageDataToChatMessageModelMapper, IHandleChatMessages chatMessagesService, IMapEntities<Collection.Messages, ChatMessageModel> collectionMessagesToChatMessageModelMapper, IGetToolWindowAction toolWindowActionGetter, IStoreDataLocally localStorageService, IHandleUserAccountEvents userAccountEvents)
        {
            this.userProvider = userProvider;
            this.httpClient = httpClient;
            this.clipboardStorage = clipboardStorage;
            this.messageSender = messageSender;
            this.syntaxBlockUIContainerFactory = syntaxBlockUIContainerFactory;
            this.jsonSerializer = jsonSerializer;
            this.messageDataToChatMessageModelMapper = messageDataToChatMessageModelMapper;
            this.chatMessagesService = chatMessagesService;
            this.collectionMessagesToChatMessageModelMapper = collectionMessagesToChatMessageModelMapper;
            this.toolWindowActionGetter = toolWindowActionGetter;
            this.localStorageService = localStorageService;
            this.userAccountEvents = userAccountEvents;
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

        public void LogoutUser(object sender)
        {
            localStorageService.DeleteUser();
            userAccountEvents.OnLogout(sender, new UserHasLogout());
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