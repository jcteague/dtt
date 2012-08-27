// using System.Collections.Generic;
// using System.Linq;
// using System.Windows.Documents;
// using Machine.Specifications;
// using Rhino.Mocks;
// using TeamNotification_Library.Extensions;
// using TeamNotification_Library.Models;
// using TeamNotification_Library.Service.Chat.Formatters;
// using developwithpassion.specifications.rhinomocks;
// using developwithpassion.specifications.extensions;
//
//namespace TeamNotification_Test.Library.Service.Chat.Formatters
//{  
//    [Subject(typeof(PlainMessagesFormatter))]  
//    public class PlainMessagesFormatterSpecs
//    {
//        public abstract class Concern : Observes<IFormatPlainMessages, PlainMessagesFormatter>
//        {
//            Establish context = () =>
//            {
//                userIndicatorFormatter = depends.on<IFormatUserIndicator>();
//            };
//
//            protected static IFormatUserIndicator userIndicatorFormatter;
//        }
//
//        public class when_getting_the_formatted_element : Concern
//        {
//            Establish context = () =>
//            {
//                userId = 10;
//                chatMessage = new ChatMessageModel
//                    {
//                        UserId = userId,
//                        UserName = "foo user",
//                        Message = "foo message"
//                    };
//            };
//
//            protected static ChatMessageModel chatMessage;
//            protected static int userId;
//        }
//   
//        public class when_getting_the_formatted_element_and_the_last_user_that_inserted_is_the_same_as_the_actual_one : when_getting_the_formatted_element
//        {
//            Because of = () =>
//                result = sut.GetFormattedElement(chatMessage, userId);
//        
//            It should_return_a_paragraph_filled_with_the_message = () =>
//            {
//                var paragraph = ((Paragraph) result.First());
//                paragraph.GetText().ShouldEqual(chatMessage.Message);
//            };
//                
//            private static IEnumerable<Block> result;
//        }
//
//        public class when_getting_the_formatted_element_and_the_last_user_that_inserted_is_different_than_the_actual_one : when_getting_the_formatted_element
//        {
//            Establish context = () =>
//            {
//                userIndicator = "blah message";
//                var userIndicatorBold = new Bold(new Run("blah message"));
//                userIndicatorFormatter.Stub(x => x.Get(chatMessage)).Return(userIndicatorBold);
//            };
//
//            Because of = () =>
//                result = sut.GetFormattedElement(chatMessage, 99);
//
//            It should_return_a_paragraph_block_as_the_first_element_of_the_list = () =>
//            {
//                var paragraph = ((Paragraph)result.First());
//                paragraph.GetText().ShouldContain(userIndicator);
//            };
//
//            It should_return_a_paragraph_filled_with_the_message = () =>
//            {
//                var paragraph = ((Paragraph)result.First());
//                paragraph.GetText().ShouldContain(chatMessage.Message);
//            };
//
//            private static IEnumerable<Block> result;
//            private static string userIndicator;
//        }
//    }
//}
