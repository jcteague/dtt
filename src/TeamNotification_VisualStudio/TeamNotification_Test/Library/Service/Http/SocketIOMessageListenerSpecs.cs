using System;
using Machine.Specifications;
using SocketIOClient.Messages;
using TeamNotification_Library.Service.Http;
 using developwithpassion.specifications.rhinomocks;
 using developwithpassion.specifications.extensions;
using Rhino.Mocks;

namespace TeamNotification_Test.Library.Service.Http
{  
    [Subject(typeof(SocketIOMessageListener))]  
    public class SocketIOMessageListenerSpecs
    {
        public abstract class Concern : Observes<IListenToMessages<Action<string, string>>,
                                            SocketIOMessageListener>
        {
            Establish context = () =>
            {
                client = depends.on<ISubscribeToPubSub<Action<string, string>>>();
            };

            protected static ISubscribeToPubSub<Action<string, string>> client;
        }

   
        public class when_listing_to_messages_on_a_channel : Concern
        {
            Establish context = () =>
            {
                channel = "blah channel";
                action = (c, payload) =>
                             {
                                 channelAndPayload = new Tuple<string, string>(c, payload);
                             };
            };

            Because of = () =>
                sut.ListenOnChannel(channel, action,null);

            It should_subscribe_to_the_channel_with_the_subscribe_response = () =>
                client.AssertWasCalled(x => x.Subscribe(channel, sut.SubscribeResponse,null));

            It should_have_stored_the_action = () =>
            {
                sut.SubscribeResponse("foo", "bar");
                channelAndPayload.Item1.ShouldEqual("foo");
                channelAndPayload.Item2.ShouldEqual("bar");
            };
            
            private static string channel;
            private static MessageReceivedAction action;
            private static Tuple<string, string> channelAndPayload;
        }
    }
}
