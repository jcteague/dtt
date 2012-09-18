 using System;
 using Machine.Specifications;
 using Rhino.Mocks;
 using TeamNotification_Library.Models;
 using TeamNotification_Library.Service.Clipboard;
 using TeamNotification_Library.Service.Http;
 using developwithpassion.specifications.rhinomocks;
 using developwithpassion.specifications.extensions;

namespace TeamNotification_Test.Library.Service.Clipboard
{  
    [Subject(typeof(ClipboardDataStorageService))]  
    public class ClipboardDataStorageServiceSpecs
    {
        public abstract class Concern : Observes<IStoreClipboardData,
                                            ClipboardDataStorageService>
        {
            Establish context = () =>
            {
                systemClipboardHandler = depends.on<IHandleSystemClipboard>();
                jsonSerializer = depends.on<ISerializeJSON>();
                                        
            };

            protected static ISerializeJSON jsonSerializer;
            protected static IHandleSystemClipboard systemClipboardHandler;
        }

        public class when_getting_the_data_and_there_is_valid_data_in_the_clipboard : Concern
        {
            Establish context = () =>
            {
                chatMessage = new ChatMessageModel
                {
                    chatMessageBody = new ChatMessageBody{
                        message = "blah"
                    }
                };

                var serializedData = "foo serialized data";
                systemClipboardHandler.Stub(x => x.GetText()).Return(serializedData);

                jsonSerializer.Stub(x => x.Deserialize<ChatMessageModel>(serializedData)).Return(chatMessage);
            };

            Because of = () =>
                result = sut.Get<ChatMessageModel>();

            It should_return_the_deserialized_data = () =>
                result.ShouldEqual(chatMessage);

            private static ChatMessageModel result;
            private static ChatMessageModel chatMessage;
        }

        public class when_getting_the_data_and_there_is_no_valid_data_in_the_clipboard : Concern
        {
            Establish context = () =>
            {
                message = "blah";
                systemClipboardHandler.Stub(x => x.GetText()).Return(message);

                jsonSerializer.Stub(x => x.Deserialize<ChatMessageModel>(message)).Throw(new Exception());
            };

            Because of = () =>
                result = sut.Get<ChatMessageModel>();

            It should_return_chat_message_data_with_the_clipboard_text = () =>
            {
                result.ShouldBeOfType<ChatMessageModel>();
                result.chatMessageBody.message.ShouldEqual(message);
            };

            private static ChatMessageModel result;
            private static string message;
        }

        public class when_getting_the_data_and_there_is_a_whitespace_string_in_the_clipboard : Concern
        {
            Establish context = () =>
            {
                message = "";
                jsonSerializer.Stub(x => x.Serialize(Arg<ChatMessageBody>.Is.Anything)).Return("{message:''}");
                systemClipboardHandler.Stub(x => x.GetText()).Return(message);
            };

            Because of = () =>
                result = sut.Get<ChatMessageModel>();

            It should_return_chat_message_data_with_the_clipboard_text = () =>
            {
                result.ShouldBeOfType<ChatMessageModel>();
                result.chatMessageBody.message.ShouldEqual(message);
            };

            private static ChatMessageModel result;
            private static string message;
        }
    }
}
