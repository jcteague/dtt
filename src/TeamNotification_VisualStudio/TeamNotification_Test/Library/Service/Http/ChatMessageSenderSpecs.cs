using Machine.Specifications;
using TeamNotification_Library.Service.Http;
using developwithpassion.specifications.rhinomocks;
using Machine.Fakes;
using developwithpassion.specifications.extensions;

namespace TeamNotification_Test.Library.Service.Http
{
    [Subject(typeof(ChatMessageSender))]
    public class chant_message_sender_specs 
    {
        public class concern : Observes<ISendChatMessages,ChatMessageSender>
        {
            
        }
        
        public class when_sending_a_message : concern
        {
            Establish context = () =>
                                    {
                                        httpRequestsClient = depends.on<ISendHttpRequests>();
                                        message = "blah";
                                        url = "http://dtt.local:3000/registration?&userName=Raymi&userMessage=blah";
                                    };

            Because b = () => sut.SendMessage(message);

            It should_send_the_message_in_the_url_using_the_client = () =>
                httpRequestsClient.WasToldTo(x => x.Get(url));

            private static ISendHttpRequests httpRequestsClient;
            private static string message;
            private static string url;
        }
    }
}