using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Input;
using Machine.Specifications;
using Rhino.Mocks;
using TeamNotification_Library.Models;
using TeamNotification_Library.Models.UI;
using TeamNotification_Library.Service.Chat.Formatters;
using TeamNotification_Library.Service.Controls;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.Providers;
using developwithpassion.specifications.rhinomocks;

namespace TeamNotification_Test.Library.Service.Controls
{
    [Subject(typeof(MessagesEditor))]
    public class MessagesEditorSpecs
    {
        public abstract class Concern : Observes<IEditMessages, MessagesEditor>
        {
            public class MouseButtonEventArgsStub : MouseButtonEventArgs
            {
                public MouseButtonEventArgsStub(MouseDevice mouse, int timestamp, MouseButton button) : base(mouse, timestamp, button) { }
                public MouseButtonEventArgsStub(MouseDevice mouse, int timestamp, MouseButton button, StylusDevice stylusDevice) : base(mouse, timestamp, button, stylusDevice) { }
                public new int ClickCount { get; set; }
            }

            private Establish context = () =>
            {
                jsonSerializer = depends.on<ISerializeJSON>();
                userProvider = depends.on<IProvideUser>();
            };

            protected static ISerializeJSON jsonSerializer;
            protected static IProvideUser userProvider;
        }

        public class when_setting_a_tablerowgroup : Concern
        {
            private Establish context = () =>
            {
                var fakeBody = "some fake value";
                rowMock = fake.an<TableRowGroup>();
                messageMock = fake.an<Collection.Messages>();
                messageMock.data = new List<CollectionData>
                                       {
                                           new CollectionData {name = "body", value = fakeBody}
                                       };
                var chatMessageBodyFake = new ChatMessageBody{message = "fake message"};
                jsonSerializer.Stub(x => x.Deserialize<ChatMessageBody>(fakeBody)).Return(chatMessageBodyFake);

                messagesContainerMock = fake.an<ChatUIElements>();
                messagesContainerMock.MessagesList = new Dictionary<string, TableRowGroup>();
                resourceName = "originalMessage";              
            };
            Because of = () =>
                sut.ConfigTableRowGroup(rowMock, messageMock, messagesContainerMock);

            It should_have_set_the_row_resource = () =>
                rowMock.Resources[resourceName].ShouldEqual(messageMock);

            private static TableRowGroup rowMock;
            private static Collection.Messages messageMock;
            private static ChatUIElements messagesContainerMock;
            private static string resourceName;
        }

        public abstract class when_the_editing_messages_event_is_called : Concern
        {
            protected Establish context = () =>
            {
            };

            protected static MouseButtonEventArgsStub mouseButtonEventArgsStub;
            protected static TableRowGroup tableRowGroup;

        }

        public class and_two_clicks_were_made : when_the_editing_messages_event_is_called
        {
            private Establish context = () =>{
                mouseButtonEventArgsStub = new MouseButtonEventArgsStub(Mouse.PrimaryDevice, 100, MouseButton.Left) {ClickCount = 2};
                tableRowGroup =  new TableRowGroup();
                //mouseButtonEventArgsMock.Stub(x => x.ClickCount).Return(2);
            };

            private Because of = () => {
                sut.EditMessage(tableRowGroup, mouseButtonEventArgsStub);
                editingMessage = sut.editingMessage;
            };
            It should_have_set_the_current_editing_message = () => editingMessage.ShouldNotBeNull();

            private static Collection.Messages editingMessage;
        }
    }
}
