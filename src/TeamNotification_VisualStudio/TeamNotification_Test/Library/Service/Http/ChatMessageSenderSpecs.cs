using Machine.Specifications;
using TeamNotification_Library.Service.Http;
using developwithpassion.specifications.rhinomocks;
using Machine.Fakes;
using developwithpassion.specifications.extensions;

namespace TeamNotification_Test.Library.Service.Http
{
    [Subject(typeof(ChatMessageSender))]
    public class ChatMessageSenderSpecs 
    {
        public class Concern : Observes<ISendChatMessages,ChatMessageSender>
        {
            
        }
        
        public class when_sending_a_message : Concern
        {
            Establish context = () =>
            {
                httpRequestsClient = depends.on<ISendHttpRequests>();
                message = "blah";
                url = "http://dtt.local:3000/room/1/messages";
            };

            Because b = () => sut.SendMessage(message, "1");

            It should_send_the_message_in_the_url_using_the_client = () =>
                httpRequestsClient.WasToldTo(x => x.Get(url));

            private static ISendHttpRequests httpRequestsClient;
            private static string message;
            private static string url;
        }
    }
}