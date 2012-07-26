using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using AvenidaSoftware.TeamNotification_Package;
using Machine.Specifications;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Factories;
using TeamNotification_Library.Service.Http;
using developwithpassion.specifications.extensions;
using developwithpassion.specifications.rhinomocks;
using Machine.Fakes;
using Rhino.Mocks;

namespace TeamNotification_Test.Package
{
    [Subject(typeof(DynamicControlBuilder))]
    public class DynamicControlBuilderSpecs
    {
//        public class Concern : Observes<IBuildDynamicControls, DynamicControlBuilder>
//        {
//             
//        }
//
//        public class when_getting_the_content_from_a_url : Concern
//        {
//            Establish context = () =>
//            {
//                httpRequestsClient = depends.on<ISendHttpRequests>();
//                callbackFactory = depends.on<ICreateCallback>();
//                uri = "blah";
//                action = (x) => { return; };
//                callbackFactory.Stub(x => x.BuildFor(Arg<StackPanel>.Is.Anything)).Return(action);
//            };
//
//            Because of = () =>
//                result = sut.GetContentFrom(uri);
//
//            It should_return_an_instance_of_a_StackPanel = () =>
//                result.ShouldBeAn<StackPanel>();
//
//            It should_make_a_get_request_to_the_client_with_a_callback_for_a_panel = () =>
//                httpRequestsClient.AssertWasCalled(x => x.Get(uri, action));
//
//            private static Panel result;
//            private static Panel expectedResult;
//            private static ISendHttpRequests httpRequestsClient;
//            private static ICreateCallback callbackFactory;
//            private static Action<Collection> action;
//            private static string uri;
//        }
    }
}