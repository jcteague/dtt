using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Models;
using TeamNotification_Library.Models.UI;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.Async.Models;
using TeamNotification_Library.Service.Chat;
using TeamNotification_Library.Service.Clipboard;
using TeamNotification_Library.Service.Factories.UI.Highlighters;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.Mappers;
using TeamNotification_Library.Service.Providers;
using TeamNotification_Library.Service.ToolWindow;
using TeamNotification_Library.UI.Avalon;

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
        private IHandleMixedEditorEvents mixedEditorEvents;

        readonly ICreateSyntaxBlockUIInstances syntaxBlockUIContainerFactory;
        private readonly ISendChatMessages messageSender;
        private IEditMessages messagesEditor;

        public ChatRoomsControlService(IProvideUser userProvider, ISendHttpRequests httpClient, IProvideConfiguration<ServerConfiguration> configuration, IStoreClipboardData clipboardStorage, ISendChatMessages messageSender, ICreateSyntaxBlockUIInstances syntaxBlockUIContainerFactory, ISerializeJSON jsonSerializer, IHandleChatMessages chatMessagesService, IMapEntities<Collection.Messages, ChatMessageModel> collectionMessagesToChatMessageModelMapper, IGetToolWindowAction toolWindowActionGetter, IStoreDataLocally localStorageService, IHandleUserAccountEvents userAccountEvents, IEditMessages messagesEditor, IHandleMixedEditorEvents mixedEditorEvents)
        {
            this.userProvider = userProvider;
            this.httpClient = httpClient;
            this.clipboardStorage = clipboardStorage;
            this.messageSender = messageSender;
            this.syntaxBlockUIContainerFactory = syntaxBlockUIContainerFactory;
            this.jsonSerializer = jsonSerializer;
            this.messagesEditor = messagesEditor;
            this.mixedEditorEvents = mixedEditorEvents;
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


        public void HandlePaste(MixedTextEditor textBox, IShowCode codeShower, DataWasPasted pasteData)
        {
            var chatMessageModel = clipboardStorage.Get<ChatMessageModel>();

            if (chatMessageModel.chatMessageBody.IsCode)
            {
                var pastedCode = codeShower.Show(chatMessageModel.chatMessageBody.message, chatMessageModel.chatMessageBody.programminglanguage);
                if(pastedCode.Trim() != "")
                {
                    chatMessageModel.chatMessageBody.message = pastedCode;
                    mixedEditorEvents.OnCodeAppended(this, new CodeWasAppended(chatMessageModel));
                }
            }
            else
            {
                mixedEditorEvents.OnTextAppended(this, new TextWasAppended(pasteData.Text));
            }
        }

        public void HandlePaste(RichTextBox textBox, IShowCode codeShower, DataObjectPastingEventArgs dataObjectPastingEventArgs)
        {
            var chatMessageModel = clipboardStorage.Get<ChatMessageModel>();
            dataObjectPastingEventArgs.CancelCommand();

            if (chatMessageModel.chatMessageBody.IsCode)
            {
                var pastedCode = codeShower.Show(chatMessageModel.chatMessageBody.message, chatMessageModel.chatMessageBody.programminglanguage);
                if(pastedCode.Trim() != "")
                {
                    chatMessageModel.chatMessageBody.message = pastedCode;
                    var block = syntaxBlockUIContainerFactory.Get(chatMessageModel);
                    textBox.Dispatcher.Invoke(new Action( () =>{
                        textBox.Document.Blocks.Clear();
                        textBox.Document.Blocks.Add(block);
                        textBox.CaretPosition = textBox.Document.ContentEnd;
                    }));
                }else
                {
                    textBox.Dispatcher.Invoke(new Action(() => textBox.Document.Blocks.Clear()));
                    dataObjectPastingEventArgs.CancelCommand();
                }
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

        public void SendMessages(IEnumerable<ChatMessageBody> messages, string roomId)
        {
            this.messageSender.SendMessages(messages,roomId);
        }

        public void ResetContainer(ChatUIElements messagesContainer)
        {
            messagesContainer.LastStamp = "";
            messagesContainer.MessagesTable.Dispatcher.Invoke( new Action(()=>messagesContainer.MessagesTable.RowGroups.Clear()));
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

        public ChatMessageModel AddReceivedMessage(ChatUIElements messagesContainer, ScrollViewer scrollviewer, string messageData)
        {
            var chatMessageModel = jsonSerializer.Deserialize<ChatMessageModel>(messageData);
            
            var rowGroup = chatMessagesService.AppendMessage(messagesContainer, scrollviewer, chatMessageModel);
            var collectionMessage = ChatMessageModelToCollectionMessage(chatMessageModel);

            if(chatMessageModel.chatMessageBody.IsCode)
                messagesEditor.ConfigTableRowGroup(rowGroup, collectionMessage, messagesContainer);
            return chatMessageModel;
        }

        public ChatRoomInvitation AddInvitedRoom(ChatUIElements messagesContainer, string invitationData)
        {
            var chatRoomInvitation = jsonSerializer.Deserialize<ChatRoomInvitation>(invitationData);

            messagesContainer.ComboRooms.Dispatcher.Invoke(new Action(() =>
            {
                var itemsSource = (List<Collection.Link>)messagesContainer.ComboRooms.ItemsSource;
                itemsSource.Add(new Collection.Link { name = chatRoomInvitation.chat_room_name, rel = chatRoomInvitation.chat_room_id });
                messagesContainer.ComboRooms.ItemsSource = itemsSource;
                messagesContainer.ComboRooms.Items.Refresh();
            }));
            return chatRoomInvitation;
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
