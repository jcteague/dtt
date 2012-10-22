 using System.Collections.Generic;
 using System.Linq;
 using System.Windows.Documents;
 using Machine.Specifications;
 using Rhino.Mocks;
 using TeamNotification_Library.Extensions;
 using TeamNotification_Library.Models;
 using TeamNotification_Library.Service.Chat.Formatters;
 using developwithpassion.specifications.rhinomocks;
 using developwithpassion.specifications.extensions;

namespace TeamNotification_Test.Library.Service.Chat.Formatters
{  
    [Subject(typeof(PlainMessagesFormatter))]  
    public class PlainMessagesFormatterSpecs
    {
        public abstract class Concern : Observes<IFormatPlainMessages, PlainMessagesFormatter>
        {
            Establish context = () =>
            {
                messageLinksParser = depends.on<IParseMessagesForLinks>();
            };

            protected static IParseMessagesForLinks messageLinksParser;
        }

        public class when_getting_the_formatted_element : Concern
        {
            Establish context = () =>
            {
                chatMessage = new ChatMessageModel
                    {
                        user_id = "9",
                        username = "foo user",
                        chatMessageBody = new ChatMessageBody { message = "foo message" }
                    };

                parsedMessage = "parsed foo message";
                var parsedSpan = new Span(new Run(parsedMessage));
                messageLinksParser.Stub(x => x.Parse(chatMessage.chatMessageBody.message)).Return(parsedSpan);
            };

            Because of = () =>
                result = sut.GetFormattedElement(chatMessage);

            It should_return_a_paragraph_filled_with_the_message = () =>
            {
                var paragraph = ((Paragraph) result);
                paragraph.GetText().ShouldEqual(parsedMessage);
            };

            private static Block result;
            private static ChatMessageModel chatMessage;
            private static string parsedMessage;
        }
    }
}
