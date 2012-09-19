 using System;
 using System.Windows.Documents;
 using Machine.Specifications;
 using TeamNotification_Library.Models;
 using TeamNotification_Library.Service.Chat.Formatters;
 using developwithpassion.specifications.rhinomocks;
 using developwithpassion.specifications.extensions;
 using TeamNotification_Library.Extensions;

namespace TeamNotification_Test.Library.Service.Chat.Formatters
{  
    [Subject(typeof(DateTimeFormatter))]  
    public class DateTimeFormatterSpecs
    {
        public abstract class Concern : Observes<IFormatDateTime,
                                            DateTimeFormatter>
        {
        
        }

   
        public abstract class when_getting_the_formatted_element : Concern
        {
        }

        public class when_getting_the_formatted_element_and_the_date_is_today : when_getting_the_formatted_element
        {
            Establish context = () =>
            {
                dateTime = DateTime.Now;
                chatMessage = new ChatMessageModel { chatMessageBody = new ChatMessageBody { date = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 11, 01, 0).ToString() } };
            };

            Because of = () =>
                result = sut.GetFormattedElement(chatMessage);

            It should_return_a_string_with_the_time = () =>
                result.GetText().ShouldEqual("11:01 AM");

            private static Paragraph result;
            private static ChatMessageModel chatMessage;
            private static DateTime dateTime;
        }

        public class when_getting_the_formatted_element_and_the_date_is_older_than_today : when_getting_the_formatted_element
        {
            Establish context = () =>
            {
                dateTime = DateTime.Now;
                chatMessage = new ChatMessageModel { chatMessageBody = new ChatMessageBody { date = new DateTime(dateTime.Year - 10, dateTime.Month, dateTime.Day, 11, 01, 0).ToString() } };
            };

            Because of = () =>
                result = sut.GetFormattedElement(chatMessage);

            It should_return_a_string_with_the_time = () =>
                result.GetText().ShouldEqual(DateTime.Parse(chatMessage.date).ToShortDateString());

            private static Paragraph result;
            private static ChatMessageModel chatMessage;
            private static DateTime dateTime;
        }
    }
}
