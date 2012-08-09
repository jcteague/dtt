using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvenidaSoftware.TeamNotification_Package;
using EnvDTE;
using Machine.Specifications;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.Async.Models;
using TeamNotification_Library.Service.Controls;
using TeamNotification_Library.Service.Factories;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.Providers;
using developwithpassion.specifications.rhinomocks;
using Rhino.Mocks;

namespace TeamNotification_Test.Library.Service.Controls
{
    [Subject(typeof(ChatRoomsControlService))]
    public class ChatRoomsControlServiceSpecs
    {
        public abstract class Concern : Observes<IServiceChatRoomsControl, ChatRoomsControlService>
        {
            Establish context = () =>
            {
                userProvider = depends.on<IProvideUser>();
                configuration = depends.on<IProvideConfiguration<ServerConfiguration>>();
                httpClient = depends.on<ISendHttpRequests>();
                clipboardEvents = depends.on<IHandleClipboardEvents>();
                clipboardArgsFactory = depends.on<ICreateClipboardArguments>();
            };

            protected static IProvideUser userProvider;
            protected static IProvideConfiguration<ServerConfiguration> configuration;
            protected static ISendHttpRequests httpClient;
            protected static IHandleClipboardEvents clipboardEvents;
            protected static ICreateClipboardArguments clipboardArgsFactory;
        }

        public class When_getting_a_collection : Concern
        {
            private Establish Context = () =>
            {
                string siteUrl = "MyUrl/";
                roomId = "1";
                IStoreConfiguration serverConfiguration = fake.an<IStoreConfiguration>();
                collection = new Collection { href = "asd" };
                messagesCollection = new Collection { href = "4509u245j" };
                User user = new User { first_name = "foo", last_name = "bar", id = 1, email = "foo@bar.com", password = "123456789" };

                Task<Collection> firstTask = Task.Factory.StartNew(() => collection);
                Task<Collection> messageTask = Task.Factory.StartNew(() => messagesCollection);

                string userUrl = siteUrl + "user/" + user.id;
                string messagesUrl = siteUrl + "room/" + roomId + "/messages";

                serverConfiguration.Stub(x => x.HREF).Return(siteUrl);
                userProvider.Stub(x => x.GetUser()).Return(user);
                configuration.Stub(x => x.Get()).Return(serverConfiguration);

                httpClient.Stub(x => x.Get<Collection>(userUrl)).Return(firstTask);
                httpClient.Stub(x => x.Get<Collection>(messagesUrl)).Return(messageTask);
            };

            private Because of = () =>
            {
                result = sut.GetCollection();
                result2 = sut.GetMessagesCollection(roomId);
            }; 

            It should_return_the_collection_from_the_http_client = () =>
                result.ShouldEqual(collection);

            It should_return_the_messages_collection_from_the_http_client = () =>
                result2.ShouldEqual(messagesCollection);

            private static string roomId;
            private static Collection result;
            private static Collection result2;
            private static Collection collection;
            private static Collection messagesCollection;
        }

        // TODO: Find a way to mock DTE to test implementation
//        public class when_updating_the_clipboard : Concern
//        {
//            Establish context = () =>
//            {
//                // Cannot mock DTE
//                dte = fake.an<DTE>();
//                chat = "blah chat";
//
//                var solutionObj = fake.an<Solution>();
//                dte.Stub(x => x.Solution).Return(solutionObj);
//
//                var solution = "blah solution";
//                solutionObj.Stub(x => x.FullName).Return(solution);
//                
//                var activeDocument = fake.an<Document>();
//                dte.Stub(x => x.ActiveDocument).Return(activeDocument);
//
//                var document = "foo document";
//                activeDocument.Stub(x => x.FullName).Return(document);
//
//                var textDocument = fake.an<TextDocument>();
//                activeDocument.Stub(x => x.Object()).Return(textDocument);
//
//                var selection = fake.an<TextSelection>();
//                textDocument.Stub(x => x.Selection).Return(selection);
//
//                var text = "foo bar text";
//                selection.Stub(x => x.Text).Return(text);
//
//                var line = 10;
//                selection.Stub(x => x.CurrentLine).Return(line);
//
//                args = new ClipboardHasChanged
//                           {
//                               Solution = solution,
//                               Document = document,
//                               Line = line,
//                               Text = text
//                           };
//                clipboardArgsFactory.Stub(x => x.Get(solution, document, text, line)).Return(args);
//            };
//
//            Because of = () =>
//                sut.UpdateClipboard(chat, dte);
//
//            It should_trigger_the_clipboard_changed_event = () =>
//                clipboardEvents.AssertWasCalled(x => x.OnClipboardChanged(chat, args));
//
//            private static ClipboardHasChanged args;
//            private static object chat;
//            private static DTE dte;
//        }
    }
}
