//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using AvenidaSoftware.TeamNotification_Package;
//using EnvDTE;
//using Machine.Specifications;
//using TeamNotification_Library.Configuration;
//using TeamNotification_Library.Models;
//using TeamNotification_Library.Service.Async;
//using TeamNotification_Library.Service.Async.Models;
//using TeamNotification_Library.Service.Chat;
//using TeamNotification_Library.Service.Clipboard;
//using TeamNotification_Library.Service.Controls;
//using TeamNotification_Library.Service.Factories;
//using TeamNotification_Library.Service.Factories.UI;
//using TeamNotification_Library.Service.Http;
//using TeamNotification_Library.Service.Mappers;
//using TeamNotification_Library.Service.Providers;
//using developwithpassion.specifications.rhinomocks;
//using Rhino.Mocks;
//using TextRange = System.Windows.Documents.TextRange;
//
//namespace TeamNotification_Test.Library.Service.Controls
//{
//    [Subject(typeof(ChatRoomsControlService))]
//    public class ChatRoomsControlServiceSpecs
//    {
//        public abstract class Concern : Observes<IServiceChatRoomsControl, ChatRoomsControlService>
//        {
//            Establish context = () =>
//            {
//                userProvider = depends.on<IProvideUser>();
//                configuration = depends.on<IProvideConfiguration<ServerConfiguration>>();
//                httpClient = depends.on<ISendHttpRequests>();
//                clipboardEvents = depends.on<IHandleClipboardEvents>();
//                clipboardArgsFactory = depends.on<ICreateClipboardArguments>();
//                clipboardDataStorageService = depends.on<IStoreClipboardData>();
//                chatMessageSender = depends.on<ISendChatMessages>();
//                jsonSerializer = depends.on<ISerializeJSON>();
//                chatMessageDataFactory = depends.on<ICreateChatMessageData>();
//                syntaxBlockUIFactory = depends.on<ICreateSyntaxBlockUIInstances>();
//                chatMessagesService = depends.on<IHandleChatMessages>();
//                collectionMessagesToChatMessageModelMapper = depends.on<IMapEntities<Collection.Messages, ChatMessageModel>>();
//                messageDataToChatMessageModelMapper = depends.on<IMapEntities<MessageData, ChatMessageModel>>();
//            };
//
//            protected static IProvideUser userProvider;
//            protected static IProvideConfiguration<ServerConfiguration> configuration;
//            protected static ISendHttpRequests httpClient;
//            protected static IHandleClipboardEvents clipboardEvents;
//            protected static ICreateClipboardArguments clipboardArgsFactory;
//            protected static IStoreClipboardData clipboardDataStorageService;
//            protected static ISendChatMessages chatMessageSender;
//            protected static ISerializeJSON jsonSerializer;
//            protected static ICreateChatMessageData chatMessageDataFactory;
//            protected static ICreateSyntaxBlockUIInstances syntaxBlockUIFactory;
//            protected static IHandleChatMessages chatMessagesService;
//            protected static IMapEntities<Collection.Messages, ChatMessageModel> collectionMessagesToChatMessageModelMapper;
//            protected static IMapEntities<MessageData, ChatMessageModel> messageDataToChatMessageModelMapper;
//        }
//
//        public abstract class when_getting_a_collection_context : Concern
//        {
//            private Establish Context = () =>
//            {
//                string siteUrl = "MyUrl/";
//                roomId = "1";
//
//                collectionMessage1 = new Collection.Messages {data = new Collection<CollectionData>{new CollectionData{name = "foo message"}}};
//                collectionMessage2 = new Collection.Messages {data = new Collection<CollectionData> { new CollectionData { name = "bar message" } } };
//
//                var serverConfiguration = fake.an<IStoreConfiguration>();
//                collection = new Collection { href = "asd" };
//                messagesCollection = new Collection { href = "4509u245j", messages = new List<Collection.Messages> { collectionMessage1, collectionMessage2 } };
//                User user = new User { first_name = "foo", last_name = "bar", id = 1, email = "foo@bar.com", password = "123456789" };
//
//                Task<Collection> firstTask = Task.Factory.StartNew(() => collection);
//                Task<Collection> messageTask = Task.Factory.StartNew(() => messagesCollection);
//
//                string userUrl = siteUrl + "user/" + user.id;
//                string messagesUrl = siteUrl + "room/" + roomId + "/messages";
//
//                serverConfiguration.Stub(x => x.Uri).Return(siteUrl);
//                userProvider.Stub(x => x.GetUser()).Return(user);
//                configuration.Stub(x => x.Get()).Return(serverConfiguration);
//
//                httpClient.Stub(x => x.Get<Collection>(userUrl)).Return(firstTask);
//                httpClient.Stub(x => x.Get<Collection>(messagesUrl)).Return(messageTask);
//            };
//
//            protected static string roomId;
//            protected static Collection result;
//            protected static Collection result2;
//            protected static Collection collection;
//            protected static Collection messagesCollection;
//            protected static Collection.Messages collectionMessage1;
//            protected static Collection.Messages collectionMessage2;
//        }
//
//        public class When_getting_a_collection : when_getting_a_collection_context
//        {
//            Because of = () =>
//                result = sut.GetCollection();
//
//            It should_return_the_collection_from_the_http_client = () =>
//                result.ShouldEqual(collection);
//        }
//
//        public class When_getting_a_collection_of_messages : when_getting_a_collection_context
//        {
//            Because of = () =>
//                result = sut.GetMessagesCollection(roomId);
//
//            It should_return_the_messages_collection_from_the_http_client = () =>
//                result.ShouldEqual(messagesCollection);
//        }
//
//        public abstract class when_handling_the_paste : Concern
//        {
//            Establish context = () =>
//            {
//                textBox = new RichTextBox();
//                textBox.Document.Blocks.Clear();
//                args = new DataObjectPastingEventArgs(new DataObject(DataFormats.Text, "foo"), false, DataFormats.Text);
//            };
//
//            protected static RichTextBox textBox;
//            protected static DataObjectPastingEventArgs args;
//        }
//
//        public class when_handling_the_paste_and_the_clipboard_has_code : when_handling_the_paste
//        {
//            Establish context = () =>
//            {
//                clipboardDataStorageService.Stub(x => x.IsCode).Return(true);
//
//                var clipboardData = new CodeClipboardData
//                {
//                    message = "blah message",
//                    solution = "blah solution",
//                    document = "blah document"
//                };
//                clipboardDataStorageService.Stub(x => x.Get<CodeClipboardData>()).Return(clipboardData);
//                
//                syntaxHighlightBox = new BlockUIContainer();
//                syntaxBlockUIFactory.Stub(x => x.Get(clipboardData)).Return(syntaxHighlightBox);
//            };
//
//            Because of = () =>
//                sut.HandlePaste(textBox, args);
//
//            It should_set_the_textbox_with_a_syntax_highlight_block = () =>
//                textBox.Document.Blocks.ShouldContain(syntaxHighlightBox);
//
//            private static BlockUIContainer syntaxHighlightBox;
//        }
//
//        public class when_sending_a_message : Concern
//        {
//            Establish context = () =>
//            {
//                textBox = new RichTextBox();
//                block1 = new Paragraph(new Run("Hello"));
//                textBox.Document.Blocks.Add(block1);
//                block2 = new BlockUIContainer(new UIElement());
//                textBox.Document.Blocks.Add(block2);
//
//                blocks = new List<Block> {block1, block2};
//            };
//
//            Because of = () =>
//                sut.SendMessage(textBox, roomId);
//
//            It should_send_the_messages_in_the_text_box = () => 
//                chatMessageSender.AssertWasCalled(x => x.SendMessages(textBox.Document.Blocks, roomId));
//
//            protected static RichTextBox textBox;
//            protected static string roomId;
//            private static Paragraph block1;
//            private static BlockUIContainer block2;
//            private static IEnumerable<Block> blocks;
//        }
//
//        public class when_adding_the_messages : when_getting_a_collection_context
//        {
//            Establish context = () =>
//            {
//                messageList = new RichTextBox();
//                scrollViewer = new ScrollViewer();
//
//                chatMessage1 = new ChatMessageModel {Message = "foo"};
//                collectionMessagesToChatMessageModelMapper.Stub(x => x.MapFrom(collectionMessage1)).Return(chatMessage1);
//                
//                chatMessage2 = new ChatMessageModel {Message = "bar"};
//                collectionMessagesToChatMessageModelMapper.Stub(x => x.MapFrom(collectionMessage2)).Return(chatMessage2);
//            };
//
//            Because of = () =>
//                sut.AddMessages(messageList, scrollViewer, roomId);
//
//            It should_append_each_message_in_the_collection = () =>
//            {
//                chatMessagesService.AssertWasCalled(x => x.AppendMessage(messageList, scrollViewer, chatMessage1));
//                chatMessagesService.AssertWasCalled(x => x.AppendMessage(messageList, scrollViewer, chatMessage2));
//            };
//
//            private static ChatMessageModel chatMessage1;
//            private static ChatMessageModel chatMessage2;
//            private static RichTextBox messageList;
//            private static ScrollViewer scrollViewer;
//        }
//
//        public class when_adding_a_received_message : Concern
//        {
//            Establish context = () =>
//            {
//                messageList = new RichTextBox();
//                scrollviewer = new ScrollViewer();
//                messageData = "foo message data";
//
//                var deserializedMessage = new MessageData {name = "foo name"};
//                jsonSerializer.Stub(x => x.Deserialize<MessageData>(messageData)).Return(deserializedMessage);
//                
//                chatMessage = new ChatMessageModel {Message = "foo message"};
//                messageDataToChatMessageModelMapper.Stub(x => x.MapFrom(deserializedMessage)).Return(chatMessage);
//            };
//
//            Because of = () =>
//                sut.AddReceivedMessage(messageList, scrollviewer, messageData);
//
//            It should_append_the_deserialized_message = () =>
//                chatMessagesService.AssertWasCalled(x => x.AppendMessage(messageList, scrollviewer, chatMessage));
//
//            private static string messageData;
//            private static ScrollViewer scrollviewer;
//            private static RichTextBox messageList;
//            private static ChatMessageModel chatMessage;
//        }
//
//        // TODO: Find a way to mock DTE to test implementation
////        public class when_updating_the_clipboard : Concern
////        {
////            Establish context = () =>
////            {
////                // Cannot mock DTE
////                dte = fake.an<DTE>();
////                chat = "blah chat";
////
////                var solutionObj = fake.an<Solution>();
////                dte.Stub(x => x.Solution).Return(solutionObj);
////
////                var solution = "blah solution";
////                solutionObj.Stub(x => x.FullName).Return(solution);
////                
////                var activeDocument = fake.an<Document>();
////                dte.Stub(x => x.ActiveDocument).Return(activeDocument);
////
////                var document = "foo document";
////                activeDocument.Stub(x => x.FullName).Return(document);
////
////                var textDocument = fake.an<TextDocument>();
////                activeDocument.Stub(x => x.Object()).Return(textDocument);
////
////                var selection = fake.an<TextSelection>();
////                textDocument.Stub(x => x.Selection).Return(selection);
////
////                var text = "foo bar text";
////                selection.Stub(x => x.Text).Return(text);
////
////                var line = 10;
////                selection.Stub(x => x.CurrentLine).Return(line);
////
////                args = new ClipboardHasChanged
////                           {
////                               Solution = solution,
////                               Document = document,
////                               Line = line,
////                               Text = text
////                           };
////                clipboardArgsFactory.Stub(x => x.Get(solution, document, text, line)).Return(args);
////            };
////
////            Because of = () =>
////                sut.UpdateClipboard(chat, dte);
////
////            It should_trigger_the_clipboard_changed_event = () =>
////                clipboardEvents.AssertWasCalled(x => x.OnClipboardChanged(chat, args));
////
////            private static ClipboardHasChanged args;
////            private static object chat;
////            private static DTE dte;
////        }
//    }
//}
