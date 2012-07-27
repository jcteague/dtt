using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Machine.Specifications;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service;
using TeamNotification_Library.Service.Factories;
using TeamNotification_Library.Service.Renderer;
using developwithpassion.specifications.rhinomocks;
using Rhino.Mocks;

namespace TeamNotification_Test.Library.Service.Renderer
{
    [Subject(typeof(ContentRenderer))]
    public class ContentRendererSpecs
    {
        public class Concern : Observes<IRenderContent, ContentRenderer>
        {
            
        }

        public class when_rendering_content : Concern
        {
            Establish context = () =>
            {
                collection = new Collection
                {
                    href = "/blah",
                    template = new Collection.Template
                    {
                        data = new List<Collection.CollectionData>{new Collection.CollectionData
                                                                                                       {
                                                                                                           name = "foo",
                                                                                                           value = "bar"
                                                                                                       }}
                    }
                };
                panelFactory = depends.on<ICreateInstances<StackPanel>>();
                templateRenderer = depends.on<IRenderCollectionTemplate>();

                panel = fake.an<StackPanel>();
                panelFactory.Stub(x => x.GetInstance()).Return(panel);

                panelChildren = fake.an<StubUIElementCollection>();
                panel.Stub(x => x.Children).Return(panelChildren);

                templatePanel = fake.an<StackPanel>();
                templateRenderer.Stub(x => x.RenderFor(collection)).Return(templatePanel);
            };

            Because of = () =>
                result = sut.Render(collection);

            It should_have_added_the_inputs_from_the_templates = () =>
                panelChildren.AssertWasCalled(x => x.Add(templatePanel));

            It should_return_the_create_stack_panel = () =>
                result.ShouldEqual(panel);            
            
            private static Collection collection;
            private static StackPanel result;
            private static ICreateInstances<StackPanel> panelFactory;
            private static StackPanel panel;
            private static StackPanel templatePanel;
            private static IRenderCollectionTemplate templateRenderer;
            private static UIElementCollection panelChildren;

            public class StubUIElementCollection : UIElementCollection
            {
                public StubUIElementCollection() : base(new UIElement(), null)
                {
                }
            }
        }
    }
}