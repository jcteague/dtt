using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows;
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

        public class when_sending_messages : Concern
        {
            Establish context = () =>
            {
                roomId = "1";
                var siteUrl = "MyUrl/";
                var serverConfiguration = fake.an<IStoreConfiguration>();
                serverConfiguration.Stub(x => x.Uri).Return(siteUrl);
                configuration.Stub(x => x.Get()).Return(serverConfiguration);

				var project = "foo project";
                var solution = "foo solution";
                var document = "foo document";
                var fooMessage = "foo message";
                var programmingLanguage = 10;

                var resources = new ResourceDictionary();
				resources["project"] = project;
                resources["solution"] = solution;
                resources["document"] = document;
                resources["message"] = fooMessage;
                resources["line"] = 1;
                resources["column"] = 2;
                resources["programmingLanguage"] = programmingLanguage;
                var block1 = new BlockUIContainer {Resources = resources};


                var barMessage = "bar message";
                var block2 = new Paragraph(new Run(barMessage));
                blocks = new List<Block> {block1, block2};

                url = siteUrl + "room/" + roomId + "/messages";

                var content1 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>{new KeyValuePair<string, string>("solution", solution)});
                objectToFormUrlEncodedContentMapper.Stub(
                    mapper =>
                        mapper.MapFrom(
                            Arg<CodeClipboardData>.Matches(x => x.solution == solution && x.document == document && x.message == fooMessage && x.programmingLanguage == programmingLanguage && x.project == project)
                       )
                   ).Return(content1);
                var message1 = new Tuple<string, HttpContent>(url, content1);

                var content2 = new FormUrlEncodedContent(new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("message", barMessage) });
                objectToFormUrlEncodedContentMapper.Stub(
                    mapper =>
                        mapper.MapFrom(
                            Arg<PlainClipboardData>.Matches(x => x.message == barMessage)
                       )
                   ).Return(content2);
                var message2 = new Tuple<string, HttpContent>(url, content2);

                messages = new List<Tuple<string, HttpContent>>() {message1, message2};
            };

            Because of = () =>
                sut.SendMessages(blocks, roomId);

            It should_post_the_formatted_messages_through_the_client = () =>
                httpRequestsClient.AssertWasCalled(x => x.Post(messages));

            private static string roomId;
            private static IEnumerable<Block> blocks;
            private static string url;
            private static IEnumerable<Tuple<string, HttpContent>> messages;
        }
    }
}