using System;
using System.Threading.Tasks;
using Machine.Specifications;
using TeamNotification_Library.Service.Http;
using developwithpassion.specifications.rhinomocks;

namespace TeamNotification_Test.Library.Service.Http
{
    [Subject(typeof(HttpRequestsClient))]
    public class HttpRequestClientSpecs
    {
        /*
        public class Concern : Observes<ISendHttpRequests, HttpRequestsClient>
        {
        }

        public class when_getting_a_uri_with_and_passing_a_callback : Concern
        {
            Establish context = () =>
            {

                action = (Task<string> x) => result = x.Result;
            };

            Because of = () =>
                sut.Get(uri, action);

            It should_call_the_action_with_the_result_from_the_request = () =>
            {

            };

            private static Action<Task<string>> action;
            private static string result;
        }
        */
    }
}