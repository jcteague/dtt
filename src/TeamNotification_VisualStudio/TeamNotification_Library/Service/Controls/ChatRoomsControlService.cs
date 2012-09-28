using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Models;
using TeamNotification_Library.Models.UI;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.Async.Models;
using TeamNotification_Library.Service.Chat;
using TeamNotification_Library.Service.Chat.Formatters;
using TeamNotification_Library.Service.Clipboard;
using TeamNotification_Library.Service.Factories.UI;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.Logging;
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
        private IMapEntities<Collection.Messages, ChatMessageModel> collectionMessagesToChatMessageModelMapper;
        private IGetToolWindowAction toolWindowActionGetter;
        private IHandleChatMessages chatMessagesService;
        private readonly IStoreDataLocally localStorageService;
        private readonly IHandleUserAccountEvents userAccountEvents;
        
        readonly ICreateSyntaxBlockUIInstances syntaxBlockUIContainerFactory;
        private readonly ISendChatMessages messageSender;
        private IEditMessages messagesEditor;

        public ChatRoomsControlService(IProvideUser userProvider, ISendHttpRequests httpClient, IProvideConfiguration<ServerConfiguration> configuration, IStoreClipboardData clipboardStorage, ISendChatMessages messageSender, ICreateSyntaxBlockUIInstances syntaxBlockUIContainerFactory, ISerializeJSON jsonSerializer, IHandleChatMessages chatMessagesService, IMapEntities<Collection.Messages, ChatMessageModel> collectionMessagesToChatMessageModelMapper, IGetToolWindowAction toolWindowActionGetter, IStoreDataLocally localStorageService, IHandleUserAccountEvents userAccountEvents, IEditMessages messagesEditor)
        {
            this.userProvider = userProvider;
            this.httpClient = httpClient;
            this.clipboardStorage = clipboardStorage;
            this.messageSender = messageSender;
            this.syntaxBlockUIContainerFactory = syntaxBlockUIContainerFactory;
            this.jsonSerializer = jsonSerializer;
            this.messagesEditor = messagesEditor;
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
            var chatMessageModel = clipboardStorage.Get<ChatMessageModel>();

            if (chatMessageModel.chatMessageBody.IsCode)
            {
                var block = syntaxBlockUIContainerFactory.Get(chatMessageModel);
                textBox.Document.Blocks.Add(block);
                textBox.CaretPosition = textBox.Document.ContentEnd;
                dataObjectPastingEventArgs.CancelCommand();
            }
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
            messagesEditor.inputMethod = textBox;
            if (messagesEditor.editingMessage != null)
            {
                var text = messagesEditor.inputMethod.Document.GetDocumentText();
                messagesEditor.editingMessageModel.chatMessageBody.message = text.Substring(0, text.Length - 2);
                messageSender.SendMessage(messagesEditor.editingMessageModel.chatMessageBody, roomId);
            }
            else
            {
                messageSender.SendMessages(textBox.Document.Blocks, roomId);
            }
            messagesEditor.ResetControls();

        }

        public void ResetContainer(ChatUIElements messagesContainer)
        {
            messagesContainer.LastStamp = "";
            messagesContainer.MessagesTable.RowGroups.Clear();
            messagesContainer.MessagesList.Clear();
        }

        public void AddMessages(ChatUIElements messagesContainer, ScrollViewer scrollviewer, string currentRoomId)
        {
            chatMessagesService.ResetUser();
            var collection = GetMessagesCollection(currentRoomId);
            foreach (var message in collection.messages)
            {
                var chatMessage = collectionMessagesToChatMessageModelMapper.MapFrom(message);
                chatMessagesService.AppendMessage(messagesContainer, scrollviewer, chatMessage);
                var idx = messagesContainer.MessagesTable.RowGroups.Count - 1;
                if (idx == -1) continue;
                
                var messageBody = Collection.getField(message.data, "body");
                var chatMessageBody = jsonSerializer.Deserialize<ChatMessageBody>(messageBody);
                
                if (chatMessageBody.IsCode)
                    messagesEditor.ConfigTableRowGroup(messagesContainer.MessagesTable.RowGroups[idx], message, messagesContainer);
            }
        }

        public void AddReceivedMessage(ChatUIElements messagesContainer, ScrollViewer scrollviewer, string messageData)
        {
            var chatMessageModel = jsonSerializer.Deserialize<ChatMessageModel>(messageData);

            var rowGroup = chatMessagesService.AppendMessage(messagesContainer, scrollviewer, chatMessageModel);
            var collectionMessage = ChatMessageModelToCollectionMessage(chatMessageModel);

            Container.GetInstance<ILog>().Write("Should print: {0}".FormatUsing(chatMessageModel.chatMessageBody.message));


            //if (chatMessageModel.user_id.ParseToInteger() != userProvider.GetUser().id) return;
            if(chatMessageModel.chatMessageBody.IsCode)
            //messagesContainer.LastStamp = lastStamp;
                messagesEditor.ConfigTableRowGroup(rowGroup, collectionMessage, messagesContainer);
            //if (!chatMessageModel.chatMessageBody.IsCode) lastStamp = messagesContainer.LastStamp;
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
    }
}