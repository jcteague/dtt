using System.Collections.Generic;
using System.Net.Http;
using Machine.Specifications;
using Rhino.Mocks;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.Mappers;
using developwithpassion.specifications.rhinomocks;
using Machine.Fakes;
using developwithpassion.specifications.extensions;

namespace TeamNotification_Test.Library.Service.Http
{
    [Subject(typeof(ChatMessageSender))]
    public class ChatMessageSenderSpecs
    {
        public class Concern : Observes<ISendChatMessages, ChatMessageSender>
        {
            Establish context = () =>
            {
                configuration = depends.on<IProvideConfiguration<ServerConfiguration>>();
                httpRequestsClient = depends.on<ISendHttpRequests>();
                objectToFormUrlEncodedContentMapper = depends.on<IMapPropertiesToFormUrlEncodedContent>();
            };

            protected static ISendHttpRequests httpRequestsClient;
            protected static IProvideConfiguration<ServerConfiguration> configuration;
            protected static IMapPropertiesToFormUrlEncodedContent objectToFormUrlEncodedContentMapper;
        }

        public class when_sending_a_message : Concern
        {
            Establish context = () =>
            {
                string siteUrl = "MyUrl/";
                var serverConfiguration = fake.an<IStoreConfiguration>();

                serverConfiguration.Stub(x => x.Uri).Return(siteUrl);
                configuration.Stub(x => x.Get()).Return(serverConfiguration);

                chatMessageData = new CodeClipboardData
                              {
                                  solution = "foo solution",
                                  document = "foo document",
                                  message = "foo message",
                                  line = 10
                              };

                var value1 = new KeyValuePair<string, string>("solution", "foo solution");
                var value2 = new KeyValuePair<string, string>("document", "foo document");
                var values = new List<KeyValuePair<string, string>> {value1, value2};
                
                form = new FormUrlEncodedContent(values);
                objectToFormUrlEncodedContentMapper.Stub(x => x.MapFrom(chatMessageData)).Return(form);


                url = siteUrl + "room/1/messages";
            };

            Because of = () => 
                sut.SendMessage(chatMessageData, "1");

            It should_send_the_message_in_the_url_using_the_client = () =>
                httpRequestsClient.AssertWasCalled(x => x.Post<ServerResponse>(url, form));

            private static CodeClipboardData chatMessageData;
            private static string url;
            private static FormUrlEncodedContent form;
        }
    }
}