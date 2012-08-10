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

   
        public class when_getting_the_data : Concern
        {
            Establish context = () =>
            {
                chatMessage = new ChatMessageData
                                  {
                                      message = "blah"
                                  };

                var serializedData = "foo serialized data";
                systemClipboardHandler.Stub(x => x.GetText()).Return(serializedData);

                jsonSerializer.Stub(x => x.Deserialize<ChatMessageData>(serializedData)).Return(chatMessage);
            };

            Because of = () =>
                result = sut.Get<ChatMessageData>();

            It should_return_the_deserialized_data = () =>
                result.ShouldEqual(chatMessage);

            private static ChatMessageData result;
            private static ChatMessageData chatMessage;
        }
    }
}
