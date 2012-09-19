 using System;
 using System.Collections.Generic;
 using System.Windows.Controls;
 using System.Windows.Documents;
 using Machine.Specifications;
 using TeamNotification_Library.Models;
 using TeamNotification_Library.Service.Chat;
 using TeamNotification_Library.Service.Chat.Formatters;
 using TeamNotification_Library.Service.Content;
 using developwithpassion.specifications.rhinomocks;
 using developwithpassion.specifications.extensions;
 using Rhino.Mocks;

namespace TeamNotification_Test.Library.Service.Chat
{
    //TODO: Find a way to mock the FlowDocument
//    [Subject(typeof(ChatMessagesService))]  
//    public class ChatMessagesServiceSpecs
//    {
//        public abstract class Concern : Observes<IHandleChatMessages, ChatMessagesService>
//        {
//            Establish context = () =>
//            {
//                codeMessageFormatter = depends.on<IFormatCodeMessages>();
//                plainMessageFormatter = depends.on<IFormatPlainMessages>();
//                userIndicatorFormatter = depends.on<IFormatUserIndicator>();
//                dateTimeFormatter = depends.on<IFormatDateTime>();
//                tableBuilder = depends.on<IBuildTable>();
//            };
//
//            protected static IFormatCodeMessages codeMessageFormatter;
//            protected static IFormatPlainMessages plainMessageFormatter;
//            protected static IFormatUserIndicator userIndicatorFormatter;
//            protected static IFormatDateTime dateTimeFormatter;
//            protected static IBuildTable tableBuilder;
//        }
//
//        public abstract class when_appending_a_message : Concern
//        {
//            Establish context = () =>
//            {
//                lastUserThatInserted = 1;
//                messageList = fake.an<RichTextBox>();
//                scrollViewer = fake.an<ScrollViewer>();
//
//                FlowDocument document = fake.an<FlowDocument>();
//                messageList.Stub(x => x.Document).Return(document);
//
//                documentBlocks = fake.an<BlockCollection>();
//                document.Stub(x => x.Blocks).Return(documentBlocks);
//            };
//
//            protected static ScrollViewer scrollViewer;
//            protected static RichTextBox messageList;
//            protected static int lastUserThatInserted;
//            protected static BlockCollection documentBlocks;
//        }
//   
//        public class when_appending_a_message_and_the_message_is_code : when_appending_a_message
//        {
//            Establish context = () =>
//            {
//                chatMessage = new ChatMessageModel
//                                  {
//                                      UserId = 2,
//                                      UserName = "blah name",
//                                      Message = "blah message",
//                                      Solution = "blah solution",
//                                      Document = "blah document",
//                                      Column = 1,
//                                      Line = 2,
//                                      DateTime = DateTime.Now,
//                                      programminglanguage = 1
//                                  };
//
//                block1 = new Paragraph(new Run("foo paragraph"));
//                block2 = new Paragraph(new Run("bar paragraph"));
//                var blocks = new List<Block>{block1, block2};
//                codeMessageFormatter.Stub(x => x.GetFormattedElement(chatMessage, lastUserThatInserted)).Return(blocks);
//            };
//
//            Because of = () =>
//                sut.AppendMessage(messageList, scrollViewer, chatMessage);
//
//            It should_add_each_formatted_element_to_the_message_list = () =>
//            {
//                documentBlocks.AssertWasCalled(x => x.Add(block1));
//                documentBlocks.AssertWasCalled(x => x.Add(block2));
//            };
//
//            private static ChatMessageModel chatMessage;
//            private static Paragraph block1;
//            private static Paragraph block2;
//        }
//    }
}
