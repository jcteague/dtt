using System.Collections.Generic;
using Machine.Specifications;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.Async.Models;
using TeamNotification_Library.Service.Highlighters.Avalon;
using TeamNotification_Library.UI.Avalon;
using developwithpassion.specifications.rhinomocks;

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

        public class when_the_send_message_requested_is_triggered : Concern
        {
            Establish context = () =>
            {
                editorText = "this is the message";
                contentResource = new SortedList<int, object>() {{1, "anything"}};
                eventArgs = new SendMessageRequestedWasRequested(editorText, null);
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

            private static SendMessageRequestedWasRequested eventArgs;
            private static string editorText;
            private static SortedList<int, object> contentResource;
        }

        public class when_the_append_code_is_triggered : Concern
        {
            Establish context = () =>
            {
                messageText = "message text\nhello there.";
                var programmingLanguage = 2;
                var chatMessageModel = new ChatMessageModel {chatMessageBody = new ChatMessageBody {message = messageText, programminglanguage = programmingLanguage}};
                eventArgs = new CodeWasAppended(chatMessageModel);

                var splittedMessage = messageText.Split('\n');
                expectedContent = new SortedList<int, object>();
                expectedContent.Add(1, new MixedEditorMessageContentAndProgrammingLanguage(splittedMessage[0], programmingLanguage));
                expectedContent.Add(2, new MixedEditorMessageContentAndProgrammingLanguage(splittedMessage[1], programmingLanguage));
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
                var castedValue = (MixedEditorMessageContentAndProgrammingLanguage) value;
                var castedExpectedValue = (MixedEditorMessageContentAndProgrammingLanguage) expectedValue;
                return castedValue.Message == castedExpectedValue.Message && castedValue.ProgrammingLanguage == castedExpectedValue.ProgrammingLanguage;
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
               ((SortedList<int, object>)sut.Resources["content"]).ShouldMatch(x => (string) x[1] == (string) expectedContent[1]);
               ((SortedList<int, object>)sut.Resources["content"]).ShouldMatch(x => (string) x[2] == (string) expectedContent[2]);
            };

            private static TextWasAppended eventArgs;
            private static string messageText;
            private static SortedList<int, object> expectedContent;
        }
         
    }
}