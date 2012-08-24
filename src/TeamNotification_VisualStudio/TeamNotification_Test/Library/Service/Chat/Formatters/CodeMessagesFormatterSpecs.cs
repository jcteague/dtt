 using System.Collections.Generic;
 using System.Windows.Controls;
 using System.Windows.Documents;
 using Machine.Specifications;
 using TeamNotification_Library.Models;
 using TeamNotification_Library.Service.Chat.Formatters;
 using TeamNotification_Library.Service.Factories.UI;
 using developwithpassion.specifications.rhinomocks;
 using developwithpassion.specifications.extensions;
 using System.Linq;
 using Rhino.Mocks;
 using TeamNotification_Library.Extensions;

namespace TeamNotification_Test.Library.Service.Chat.Formatters
{  
    [Subject(typeof(CodeMessagesFormatter))]  
    public class CodeMessagesFormatterSpecs
    {
        public abstract class Concern : Observes<IFormatCodeMessages, CodeMessagesFormatter>
        {
            Establish context = () =>
            {
                syntaxBlockUIContainerFactory = depends.on<ICreateSyntaxBlockUIInstances>();
                userIndicatorFormatter = depends.on<IFormatUserIndicator>();
            };

            protected static ICreateSyntaxBlockUIInstances syntaxBlockUIContainerFactory;
            protected static IFormatUserIndicator userIndicatorFormatter;
        }

        public abstract class when_gettting_the_formatted_element : Concern
        {
            Establish context = () =>
            {
                userId = 1;
                chatMessage = new ChatMessageModel
                                  {
                                      UserId = userId,
                                      Message = "foo message",
                                      Solution = "foo solution",
                                      Document = "foo document",
                                      Line = 10,
                                      Column = 99,
                                      ProgrammingLanguage = 1
                                  };

                syntaxBlock = new BlockUIContainer(new TextBlock(new Run("hello")));

                syntaxBlockUIContainerFactory.Stub(
                    x =>
                        x.Get(Arg<CodeClipboardData>.Matches(
                            y => 
                                y.message == chatMessage.Message && 
                                y.solution == chatMessage.Solution &&
                                y.document == chatMessage.Document &&
                                y.line == chatMessage.Line &&
                                y.column == chatMessage.Column &&
                                y.programmingLanguage == chatMessage.ProgrammingLanguage
                        )
                    )
                ).Return(syntaxBlock);
            };

            protected static ChatMessageModel chatMessage;
            protected static BlockUIContainer syntaxBlock;
            protected static int userId;
            protected static CodeClipboardData clipboardData;
        }
   
        public class when_gettting_the_formatted_element_and_the_last_user_that_inserted_is_the_same_as_the_actual_one : when_gettting_the_formatted_element
        {
            Because of = () =>
                result = sut.GetFormattedElement(chatMessage, userId);

            It should_return_a_syntax_block_as_the_second_element_of_the_list = () =>
                result.ShouldContain(syntaxBlock);

            private static IEnumerable<Block> result;
        }

        public class when_gettting_the_formatted_element_and_the_last_user_that_inserted_is_different_than_the_actual_one : when_gettting_the_formatted_element
        {
            Establish context = () =>
            {
                userIndicator = "blah message";
                var userIndicatorBold = new Bold(new Run("blah message"));
                userIndicatorFormatter.Stub(x => x.Get(chatMessage)).Return(userIndicatorBold);
            };

            Because of = () =>
                result = sut.GetFormattedElement(chatMessage, 99);

            It should_return_a_paragraph_block_as_the_first_element_of_the_list = () =>
            {
                var paragraph = ((Paragraph) result.First());
                paragraph.GetText().ShouldEqual(userIndicator);
            };

            It should_return_a_syntax_block_as_the_second_element_of_the_list = () =>
                result.ShouldContain(syntaxBlock);

            private static IEnumerable<Block> result;
            private static string userIndicator;
        }
    }
}
