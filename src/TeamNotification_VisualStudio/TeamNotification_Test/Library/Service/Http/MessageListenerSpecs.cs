using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Machine.Fakes;
using Machine.Specifications;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Service;
using TeamNotification_Library.Service.Factories;
using TeamNotification_Library.Service.FileSystem;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.Providers;
using developwithpassion.specifications.rhinomocks;
using Rhino.Mocks;

namespace TeamNotification_Test.Library.Service.Http
{
    [Subject(typeof(MessageListener))]
    public class MessageListenerSpecs
    {
        public class Concern : Observes<IListenToMessages<Action<string, byte[]>>, MessageListener>
        {
        }

        public class when_asked_to_listen_to_a_channel : Concern
        {
            Establish context = () =>
            {
                iConnectToRedis = depends.on<ISubscribeToPubSub<Action<string, byte[]>>>();
                action = fake.an<MessageReceivedAction>();
                channel = "test";
                subscribeResponse = (c, bytes) =>
                    action(channel, new UTF8Encoding().GetString(bytes));
            };

            private Because of = () =>
            {
                sut.ListenOnChannel(channel, action);
                subscribeResponse = sut.SubscribeResponse;
            };

            private It should_call_the_action_with_the_result_from_the_request = () =>
                iConnectToRedis.AssertWasCalled(x => x.Subscribe(channel, subscribeResponse));

            private static ISubscribeToPubSub<Action<string, byte[]>> iConnectToRedis;
            private static MessageReceivedAction action;
            private static string channel;
            private static Action<string, byte[]> subscribeResponse;
        }
    }
}