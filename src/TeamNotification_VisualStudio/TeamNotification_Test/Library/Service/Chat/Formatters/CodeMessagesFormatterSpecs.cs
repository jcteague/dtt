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
                chatMessage = new ChatMessageModel
                                  {
                                      UserId = 9,
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

            Because of = () =>
                result = sut.GetFormattedElement(chatMessage);

            It should_return_the_syntax_block_for_that_code = () =>
                result.ShouldEqual(syntaxBlock);

            private static Block result;

            private static ChatMessageModel chatMessage;
            private static BlockUIContainer syntaxBlock;
        }
    }
}
