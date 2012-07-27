using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using AvenidaSoftware.TeamNotification_Package;
using Machine.Specifications;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Factories;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.Renderer;
using developwithpassion.specifications.extensions;
using developwithpassion.specifications.rhinomocks;
using Machine.Fakes;
using Rhino.Mocks;

namespace TeamNotification_Test.Package
{
    [Subject(typeof(DynamicControlBuilder))]
    public class DynamicControlBuilderSpecs
    {
        public class Concern : Observes<IBuildDynamicControls, DynamicControlBuilder>
        {
             
        }

        public class when_getting_the_content_from_a_url : Concern
        {
            Establish context = () =>
            {
                httpRequestsClient = depends.on<ISendHttpRequests>();
                contentRenderer = depends.on<IRenderContent>();
                uri = "blah";
                
                collection = new Collection();
                Task<Collection> task = Task.Factory.StartNew(() => collection);
                httpRequestsClient.Stub(x => x.Get<Collection>(uri)).Return(task);
                
                panel = new StackPanel();
                contentRenderer.Stub(x => x.Render(collection)).Return(panel);
            };

            Because of = () =>
                result = sut.GetContentFrom(uri);

            It should_return_a_StackPanel_for_the_collection = () =>
                result.ShouldEqual(panel);

            private static StackPanel result;
            private static ISendHttpRequests httpRequestsClient;
            private static IRenderContent contentRenderer;
            private static StackPanel panel;
            private static string uri;
            private static Collection collection;
        }
    }
}