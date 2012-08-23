using System.Collections.Generic;
using System.Net.Http;
using System.Windows.Documents;
using Machine.Specifications;
using Rhino.Mocks;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.Mappers;
using developwithpassion.specifications.rhinomocks;

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

        public abstract class when_sending_a_message : Concern
        {
            Establish context = () =>
            {
                roomId = "1";
                string siteUrl = "MyUrl/";
                var serverConfiguration = fake.an<IStoreConfiguration>();

                serverConfiguration.Stub(x => x.Uri).Return(siteUrl);
                configuration.Stub(x => x.Get()).Return(serverConfiguration);

                url = siteUrl + "room/" + roomId + "/messages";
            };

            protected static string url;
            protected static string roomId;
        }

        public class when_sending_a_message_and_the_block_is_code : when_sending_a_message
        {
            Establish context = () =>
            {
                chatMessageData = new CodeClipboardData
                {
                    project = "foo project",
                    solution = "foo solution",
                    document = "foo document",
                    message = "foo message",
                    line = 10
                };
                block = new BlockUIContainer {Resources = chatMessageData.AsResources()};

                var value1 = new KeyValuePair<string, string>("solution", "foo solution");
                var value2 = new KeyValuePair<string, string>("document", "foo document");
                var values = new List<KeyValuePair<string, string>> { value1, value2 };

                form = new FormUrlEncodedContent(values);
                objectToFormUrlEncodedContentMapper.Stub(x => x.MapFrom(Arg<CodeClipboardData>.Matches(y => y.solution == chatMessageData.solution && y.message == chatMessageData.message))).Return(form);
            };

            Because of = () =>
                sut.SendMessage(block, roomId);

            It should_send_the_message_in_the_url_using_the_client = () =>
                httpRequestsClient.AssertWasCalled(x => x.Post<ServerResponse>(url, form));

            private static FormUrlEncodedContent form;
            private static CodeClipboardData chatMessageData;
            private static BlockUIContainer block;
        }

        public class when_sending_a_message_and_the_block_is_a_paragraph : when_sending_a_message
        {
            Establish context = () =>
            {
                chatMessageData = new PlainClipboardData
                {
                    message = "foo message",
                };
                block = new Paragraph(new Run(chatMessageData.message));

                var value1 = new KeyValuePair<string, string>("message", chatMessageData.message);
                var values = new List<KeyValuePair<string, string>> { value1 };

                form = new FormUrlEncodedContent(values);
                objectToFormUrlEncodedContentMapper.Stub(x => x.MapFrom(Arg<PlainClipboardData>.Matches(y => y.message == chatMessageData.message))).Return(form);
            };

            Because of = () =>
                sut.SendMessage(block, roomId);

            It should_send_the_message_in_the_url_using_the_client = () =>
                httpRequestsClient.AssertWasCalled(x => x.Post<ServerResponse>(url, form));

            private static FormUrlEncodedContent form;
            private static PlainClipboardData chatMessageData;
            private static Paragraph block;
        }
    }
}