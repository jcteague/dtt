using System;
using Machine.Specifications;
using SocketIOClient;
using SocketIOClient.Messages;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Service.Factories;
using TeamNotification_Library.Service.Http;
using developwithpassion.specifications.rhinomocks;
using developwithpassion.specifications.extensions;
using Rhino.Mocks;

namespace TeamNotification_Test.Library.Service.Http
{  
    [Subject(typeof(SocketIOPubSubClient))]  
    public class SocketIOPubSubClientSpecs
    {
        public abstract class Concern : Observes<ISubscribeToPubSub<Action<string, string>>,
                                            SocketIOPubSubClient>
        {
            Establish context = () =>
            {
                socketIOClientFactory = depends.on<ICreateInstances<IWrapSocketIOClient>>();
                                        
            };

            protected static ICreateInstances<IWrapSocketIOClient> socketIOClientFactory;
        }

   
        public class when_subscribing_to_a_channel_with_an_action : Concern
        {
            Establish context = () =>
            {
                channel = "channel 2";
                callback = (c, payload) => channelAndPayload = new Tuple<string, string>(c, payload);

                var client = fake.an<IWrapSocketIOClient>();
                socketIOClientFactory.Stub(x => x.GetInstance()).Return(client);

                roomSocket = fake.an<IEndPointClient>();
                client.Stub(x => x.Connect("/room/2/messages")).Return(roomSocket);
            };

            Because of = () =>
                sut.Subscribe(channel, callback);

            It should_listen_the_message_event_with_the_passed_callback = () =>
                roomSocket.AssertWasCalled(x => x.On(Arg<string>.Is.Equal("message"), Arg<Action<IMessage>>.Is.Anything));

            private static Tuple<string, string> channelAndPayload;
            private static string channel;
            private static Action<string, string> callback;
            private static IEndPointClient roomSocket;
        }
    }
}
