using System.Collections.Generic;
using Machine.Specifications;
using Rhino.Mocks;
using TeamNotification_Library.Configuration;
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
                string siteUrl = "MyUrl/";
                IProvideConfiguration<ServerConfiguration> configuration = depends.on<IProvideConfiguration<ServerConfiguration>>();
                IStoreConfiguration serverConfiguration = fake.an<IStoreConfiguration>();
                httpRequestsClient = depends.on<ISendHttpRequests>();

                serverConfiguration.Stub(x => x.HREF).Return(siteUrl);
                configuration.Stub(x => x.Get()).Return(serverConfiguration);

                message = "blah";
                url = siteUrl+"room/1/messages";//"http://dtt.local:3000/room/1/messages";
                values = new KeyValuePair<string, string>("message", message);
            };

            Because b = () =>
            {
                sut.SendMessage(message, "1");
            };

            It should_send_the_message_in_the_url_using_the_client = () =>
                httpRequestsClient.WasToldTo(x => x.Post(url, values));

            private static KeyValuePair<string, string> values; 
            private static ISendHttpRequests httpRequestsClient;
            private static string message;
            private static string url;
        }
    }
}