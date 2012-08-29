 using System;
 using System.Windows.Documents;
 using Machine.Specifications;
 using TeamNotification_Library.Models;
 using TeamNotification_Library.Service.Chat.Formatters;
 using developwithpassion.specifications.rhinomocks;
 using developwithpassion.specifications.extensions;
 using Rhino.Mocks;
 using TeamNotification_Library.Extensions;

namespace TeamNotification_Test.Library.Service.Chat.Formatters
{  
    [Subject(typeof(UserMessageIndicatorFormatter))]  
    public class UserMessageIndicatorFormatterSpecs
    {
        public abstract class Concern : Observes<IFormatUserIndicator,
                                            UserMessageIndicatorFormatter>
        {
        }

        public abstract class when_getting_the_formatted_element : Concern
        {
            Establish context = () =>
            {
                userId = 10;
                chatMessage = new ChatMessageModel {UserId = userId, UserName = "Blah User"};
            };

            protected static int userId;
            protected static ChatMessageModel chatMessage;
        }

        public class when_getting_the_formatted_element_and_the_user_that_last_inserted_is_the_same : when_getting_the_formatted_element
        {
            Because of = () =>
                result = sut.GetFormattedElement(chatMessage, userId);

            It should_return_an_empty_paragraph = () =>
                result.GetText().ShouldBeEmpty();

            private static Paragraph result;
        }

        public class when_getting_the_formatted_element_and_the_user_that_last_inserted_is_not_the_same : when_getting_the_formatted_element
        {
            Because of = () =>
                result = sut.GetFormattedElement(chatMessage, 99);

            It should_return_an_empty_paragraph = () =>
                result.GetText().ShouldEqual(chatMessage.UserName);

            private static Paragraph result;
        }
    }
}
