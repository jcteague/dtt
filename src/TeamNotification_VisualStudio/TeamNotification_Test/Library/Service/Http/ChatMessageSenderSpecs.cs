using System.Collections.Generic;
using Machine.Specifications;
using Rhino.Mocks;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Http;
using developwithpassion.specifications.rhinomocks;
using Machine.Fakes;
using developwithpassion.specifications.extensions;

namespace TeamNotification_Test.Library.Service.Http
{
//    [Subject(typeof(ChatMessageSender))]
//    public class ChatMessageSenderSpecs
//    {
//        public class Concern : Observes<ISendChatMessages, ChatMessageSender>
//        {
//            Establish context = () =>
//            {
//                configuration = depends.on<IProvideConfiguration<ServerConfiguration>>();
//                httpRequestsClient = depends.on<ISendHttpRequests>();                                        
//            };
//
//            protected static ISendHttpRequests httpRequestsClient;
//            protected static IProvideConfiguration<ServerConfiguration> configuration;
//        }
//
//        public class when_sending_a_message : Concern
//        {
//            Establish context = () =>
//            {
//                string siteUrl = "MyUrl/";
//                var serverConfiguration = fake.an<IStoreConfiguration>();
//
//                serverConfiguration.Stub(x => x.Uri).Return(siteUrl);
//                configuration.Stub(x => x.Get()).Return(serverConfiguration);
//
//                chatMessageData = new CodeClipboardData
//                              {
//                                  solution = "foo solution",
//                                  document = "foo document",
//                                  message = "foo message",
//                                  line = 10
//                              };
//
//                url = siteUrl + "room/1/messages";
//                values = new KeyValuePair<string, string>("message", chatMessageData);
//            };
//
//            Because of = () => 
//                sut.SendMessage(chatMessageData, "1");
//
//            It should_send_the_message_in_the_url_using_the_client = () =>
//                httpRequestsClient.WasToldTo(x => x.Post(url, values));
//
//            private static CodeClipboardData chatMessageData;
//            private static string url;
//            private static KeyValuePair<string, string> values;
//        }
//    }
}