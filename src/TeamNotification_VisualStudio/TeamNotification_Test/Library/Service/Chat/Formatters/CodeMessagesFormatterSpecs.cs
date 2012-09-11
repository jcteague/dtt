 using System;
 using System.Collections.Generic;
 using System.Windows.Controls;
 using System.Windows.Documents;
 using AurelienRibon.Ui.SyntaxHighlightBox;
 using Machine.Specifications;
 using TeamNotification_Library.Models;
 using TeamNotification_Library.Service.Async;
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
                codePasteEvents = depends.on<IHandleCodePaste>();
                syntaxHighlightBoxFactory = depends.on<ICreateSyntaxHighlightBox>();
            };
            
            protected static IHandleCodePaste codePasteEvents;
            protected static ICreateSyntaxHighlightBox syntaxHighlightBoxFactory;
        }

        public class when_gettting_the_formatted_element : Concern
        {
            Establish context = () =>
            {
                chatMessage = new ChatMessageModel
                                  {
                                      user_id = "9",
                                      chatMessageBody = new ChatMessageBody
                                      {
                                          message = "foo message",
                                          project = "foo project",
                                          solution = "foo solution",
                                          document = "foo document",
                                          line = 10,
                                          column = 99,
                                          programminglanguage = 1
                                      }
                                  };

                syntaxBlock = new SyntaxHighlightBox { Text = chatMessage.chatMessageBody.message };
                syntaxHighlightBoxFactory.Stub(x => x.Get(chatMessage.chatMessageBody.message, chatMessage.chatMessageBody.programminglanguage)).Return(syntaxBlock);
            };

            Because of = () =>
                result = sut.GetFormattedElement(chatMessage);

            It should_return_a_paragraph_with_a_link_as_the_first_inline = () =>
            {
                var firstInline = result.Inlines.FirstInline;
                firstInline.GetText().ShouldEqual("{0} \\ {1} - Line: {2}".FormatUsing(chatMessage.chatMessageBody.project, chatMessage.chatMessageBody.document, chatMessage.chatMessageBody.line.ToString()));
            };

            It should_add_an_event_handler_for_the_click_of_the_link = () =>
            {
                var firstInline = result.Inlines.FirstInline;
                var link = (Hyperlink)firstInline;
                link.DoClick();
                codePasteEvents.AssertWasCalled(x => x.OnCodePasteClick(Arg<object>.Is.Equal(link), Arg<EventArgs>.Is.Anything));
            };

            It should_contain_a_line_break_between_the_link_and_the_syntax_box = () => 
                result.Inlines.ElementAt(1).ShouldBeAn<LineBreak>();
            
            It should_contain_the_syntax_highlight_box = () =>
            {
                var lastInline = result.Inlines.LastInline;
                ((InlineUIContainer) lastInline).Child.ShouldEqual(syntaxBlock);
            };

            private static Paragraph result;
            private static ChatMessageModel chatMessage;
            private static SyntaxHighlightBox syntaxBlock;
        }
    }
}
