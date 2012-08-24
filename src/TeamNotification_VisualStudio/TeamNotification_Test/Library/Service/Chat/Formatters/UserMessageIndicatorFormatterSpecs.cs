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
        public abstract class concern : Observes<IFormatUserIndicator,
                                            UserMessageIndicatorFormatter>
        {
            Establish context = () =>
            {
                dateTimeFormatter = depends.on<IFormatDateTime>();
            };

            protected static IFormatDateTime dateTimeFormatter;
        }

   
        public class when_formatting_a_chat_message : concern
        {
            Establish context = () =>
            {
                chatMessage = new ChatMessageModel
                    {
                        UserId = 9,
                        UserName = "foo user",
                        Message = "foo message",
                        DateTime = DateTime.Now
                    };

                dateTimeText = "blah dateTime";
                dateTimeFormatter.Stub(x => x.Format(chatMessage.DateTime)).Return(dateTimeText);
            };

            Because of = () =>
                result = sut.Get(chatMessage);

            It should_return_a_message_bolded = () =>
            {
                result.ShouldBeOfType<Bold>();
                result.GetText().ShouldEqual("{0}({1}):".FormatUsing(chatMessage.UserName, dateTimeFormatter.Format(chatMessage.DateTime)));
            };
            
                
            private static ChatMessageModel chatMessage;
            private static Bold result;
            private static string dateTimeText;
        }
    }
}
