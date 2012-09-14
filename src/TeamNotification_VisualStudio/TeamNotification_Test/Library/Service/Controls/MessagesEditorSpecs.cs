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
                rowMock = fake.an<TableRowGroup>();
                messageMock = fake.an<Collection.Messages>();
                messagesContainerMock = fake.an<MessagesContainer>();
                resourceName = "originalMessage";              
            };
            Because of = () =>
                sut.ConfigTableRowGroup(rowMock, messageMock, messagesContainerMock);

            It should_have_set_the_row_resource = () =>
                rowMock.Resources[resourceName].ShouldEqual(messageMock);

            private static TableRowGroup rowMock;
            private static Collection.Messages messageMock;
            private static MessagesContainer messagesContainerMock;
            private static string resourceName;
        }

        public abstract class when_the_editing_messages_event_is_called : Concern
        {
            protected Establish context = () =>
            {
            };

            protected static MouseButtonEventArgs mouseButtonEventArgsMock;
            protected static TableRowGroup tableRowGroupMock;

        }

        public class and_two_clicks_were_made : when_the_editing_messages_event_is_called
        {
            private Establish context = () =>
            {
                mouseButtonEventArgsMock = fake.an<MouseButtonEventArgs>();
                tableRowGroupMock = fake.an<TableRowGroup>();
                mouseButtonEventArgsMock.Stub(x => x.ClickCount).Return(2);
            };
            
            Because of = () =>
                sut.EditMessage(tableRowGroupMock, mouseButtonEventArgsMock);

            //It should_return_right_away = () =>


        }
    }
}
