using System.Collections.Generic;
using Machine.Specifications;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.Async.Models;
using TeamNotification_Library.Service.Highlighters.Avalon;
using TeamNotification_Library.UI.Avalon;
using developwithpassion.specifications.rhinomocks;
using System.Linq;

namespace TeamNotification_Test.Library.UI.Avalon
{
    [Subject(typeof(MixedTextEditor))]
    public class MixedTextEditorSpecs
    {
        public class Concern : Observes<MixedTextEditor>
        {
            Establish context = () =>
            {
                mixedEditorEvents = new MixedEditorEvents();
                chatEvents = new ChatEvents();

                depends.on(mixedEditorEvents);
                depends.on(chatEvents);
            };

            It should_contain_the_correct_text_area_input_handler = () =>
                sut.TextArea.ActiveInputHandler.ShouldBeOfType<MixedEditorTextAreaInputHandler>();

            protected static IHandleMixedEditorEvents mixedEditorEvents;
            protected static IHandleChatEvents chatEvents;
        }

        public class when_getting_the_text_editor_messages : Concern
        {
            Establish context = () =>
            {
                expectedResult = new List<ChatMessageBody>();
            };

            public class and_the_text_editor_only_has_text
            {
                Establish context = () =>
                {
                    text = "this is some message that we currently\nhave\nand it fills some lines";
                };

                Because of = () =>
                {
                    sut.Text = text;
                    result = sut.GetTextEditorMessages();
                };

                It should_return_a_list_of_the_text_in_the_result = () =>
                    result.First().message.ShouldEqual(text);

                private static string text;
            }

            public class and_the_text_editor_has_code
            {
                Establish context = () =>
                {
                    contentResource = new SortedList<int, object>();
                    var codeLine1 = new MixedEditorLineData("class A {", 1, "solution", "project", "document", 3, 4);
                    var codeLine2 = new MixedEditorLineData(" ", 1, "solution", "project", "document", 4, 4);
                    var codeLine3 = new MixedEditorLineData("\tstatic string Hello() {", 1, "solution", "project", "document", 5, 4);
                    contentResource.Add(1, codeLine1);
                    contentResource.Add(2, codeLine2);
                    contentResource.Add(3, codeLine3);

                    var chatMessageBody = new ChatMessageBody
                                              {
                                                  message = "class A {\n \n\tstatic string Hello() {",
                                                  programminglanguage = codeLine1.ProgrammingLanguage,
                                                  solution = codeLine1.Solution,
                                                  project = codeLine1.Project,
                                                  document = codeLine1.Document,
                                                  line = codeLine1.Line,
                                                  column = codeLine1.Column
                                              };
                    expectedResult.Add(chatMessageBody);
                };

                Because of = () =>
                {
                    sut.Resources.Add("content", contentResource);
                    result = sut.GetTextEditorMessages();
                };

                It should_return_a_list_of_chat_message_bodies_containing_the_code_as_one_chat_message_body = () =>
                    result.First().ShouldMatch(x => is_matching(x, expectedResult.First()));

                private static bool is_matching(ChatMessageBody value, ChatMessageBody expectedValue)
                {
                    return value.message == expectedValue.message &&
                           value.programminglanguage == expectedValue.programminglanguage &&
                           value.solution == expectedValue.solution &&
                           value.project == expectedValue.project &&
                           value.document == expectedValue.document &&
                           value.line == expectedValue.line &&
                           value.column == expectedValue.column;

                }

                private static SortedList<int, object> contentResource;
            }

            public class and_the_text_editor_has_text_and_code
            {
                Establish context = () =>
                {
                    contentResource = new SortedList<int, object>();
                    var codeLine1 = new MixedEditorLineData("class AnotherClass {", 1, "solution", "project", "document", 3, 4);
                    var codeLine2 = new MixedEditorLineData(" ", 1, "solution", "project", "document", 4, 4);
                    var codeLine3 = new MixedEditorLineData("\tstatic string Hello() {", 1, "solution", "project", "document", 5, 4);
                    contentResource.Add(1, codeLine1);
                    contentResource.Add(2, codeLine2);
                    contentResource.Add(3, codeLine3);

                    editorTextAtTimeOfCall = "class A {\n \n\tstatic string Hello() {";

                    var chatMessageBody = new ChatMessageBody
                                              {
                                                  message = editorTextAtTimeOfCall,
                                                  programminglanguage = codeLine1.ProgrammingLanguage,
                                                  solution = codeLine1.Solution,
                                                  project = codeLine1.Project,
                                                  document = codeLine1.Document,
                                                  line = codeLine1.Line,
                                                  column = codeLine1.Column
                                              };
                    expectedResult.Add(chatMessageBody);
                };

                Because of = () =>
                {
                    sut.Text = editorTextAtTimeOfCall;
                    sut.Resources.Add("content", contentResource);
                    result = sut.GetTextEditorMessages();
                };

                It should_return_a_list_of_chat_message_bodies_containing_the_code_as_one_chat_message_body = () =>
                    result.First().ShouldMatch(x => is_matching(x, expectedResult.First()));

                private static bool is_matching(ChatMessageBody value, ChatMessageBody expectedValue)
                {
                    return value.message == expectedValue.message &&
                           value.programminglanguage == expectedValue.programminglanguage &&
                           value.solution == expectedValue.solution &&
                           value.project == expectedValue.project &&
                           value.document == expectedValue.document &&
                           value.line == expectedValue.line &&
                           value.column == expectedValue.column;

                }

                private static SortedList<int, object> contentResource;
                private static string editorTextAtTimeOfCall;
            }

            private static IEnumerable<ChatMessageBody> result;
            private static IList<ChatMessageBody> expectedResult;
        }

        public class when_the_send_message_requested_is_triggered : Concern
        {
            Establish context = () =>
            {
                editorText = "this is the message";
                contentResource = new SortedList<int, object>() { { 1, "anything" } };
                eventArgs = new SendMessageWasRequested(editorText,"1");
            };

            Because of = () =>
             {
                 sut.Text = editorText;
                 sut.Resources.Add("content", contentResource);
                 chatEvents.OnSendMessageRequested(null, eventArgs);
             };

            It should_clear_the_text_in_the_editor = () =>
                sut.Text.ShouldBeEmpty();

            It should_cleat_the_resources_in_the_editor = () =>
                sut.Resources.Contains("content").ShouldBeFalse();

            private static SendMessageWasRequested eventArgs;
            private static string editorText;
            private static SortedList<int, object> contentResource;
        }

        public class when_the_append_code_is_triggered : Concern
        {
            Establish context = () =>
            {
                messageText = "message text\nhello there.";
                var programmingLanguage = 2;
                var chatMessageBody = new ChatMessageBody
                                          {
                                              message = messageText,
                                              programminglanguage = programmingLanguage,
                                              column = 0,
                                              line = 2,
                                              document = "blah document",
                                              project = "blah project",
                                              solution = "blah solution",
                                          };
                var chatMessageModel = new ChatMessageModel { chatMessageBody = chatMessageBody };
                eventArgs = new CodeWasAppended(chatMessageModel);

                var splittedMessage = messageText.Split('\n');
                expectedContent = new SortedList<int, object>();
                expectedContent.Add(1, new MixedEditorLineData(splittedMessage[0], programmingLanguage, chatMessageBody.solution, chatMessageBody.project, chatMessageBody.document, chatMessageBody.line, chatMessageBody.column));
                expectedContent.Add(2, new MixedEditorLineData(splittedMessage[1], programmingLanguage, chatMessageBody.solution, chatMessageBody.project, chatMessageBody.document, chatMessageBody.line + 1, chatMessageBody.column));
            };

            Because of = () =>
                mixedEditorEvents.OnCodeAppended(null, eventArgs);

            It should_add_the_message_to_the_text_of_the_editor = () =>
                sut.Text.ShouldContain(messageText);

            It should_add_message_to_the_resources_of_the_editor = () =>
            {
                ((SortedList<int, object>)sut.Resources["content"]).ShouldMatch(x => does_content_match(x[1], expectedContent[1]));
                ((SortedList<int, object>)sut.Resources["content"]).ShouldMatch(x => does_content_match(x[2], expectedContent[2]));
            };

            private static CodeWasAppended eventArgs;
            private static string messageText;
            private static SortedList<int, object> expectedContent;

            private static bool does_content_match(object value, object expectedValue)
            {
                var castedValue = (MixedEditorLineData)value;
                var castedExpectedValue = (MixedEditorLineData)expectedValue;
                return
                    castedValue.Message == castedExpectedValue.Message &&
                    castedValue.ProgrammingLanguage == castedExpectedValue.ProgrammingLanguage &&
                    castedValue.Solution == castedExpectedValue.Solution &&
                    castedValue.Project == castedExpectedValue.Project &&
                    castedValue.Document == castedExpectedValue.Document &&
                    castedValue.Line == castedExpectedValue.Line;
            }
        }

        public class when_the_append_text_is_triggered : Concern
        {
            Establish context = () =>
            {
                messageText = "message text\nhello there.";
                eventArgs = new TextWasAppended(messageText);

                var splittedMessage = messageText.Split('\n');
                expectedContent = new SortedList<int, object>();
                expectedContent.Add(1, splittedMessage[0]);
                expectedContent.Add(2, splittedMessage[1]);
            };

            Because of = () =>
                mixedEditorEvents.OnTextAppended(null, eventArgs);

            It should_add_the_message_to_the_text_of_the_editor = () =>
                sut.Text.ShouldContain(messageText);

            It should_add_message_to_the_resources_of_the_editor = () =>
            {
                ((SortedList<int, object>)sut.Resources["content"]).ShouldMatch(x => (string)x[1] == (string)expectedContent[1]);
                ((SortedList<int, object>)sut.Resources["content"]).ShouldMatch(x => (string)x[2] == (string)expectedContent[2]);
            };

            private static TextWasAppended eventArgs;
            private static string messageText;
            private static SortedList<int, object> expectedContent;
        }

    }
}