using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Machine.Specifications;
using Rhino.Mocks;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Factories;
using TeamNotification_Library.Service.Factories.UI;
using TeamNotification_Library.Service.Renderer;
using developwithpassion.specifications.rhinomocks;

namespace TeamNotification_Test.Library.Service.Renderer
{
    [Subject(typeof(CollectionTemplateRenderer))]
    public class CollectionTemplateRendererSpecs
    {
        public class Concern : Observes<IRenderCollectionTemplate, CollectionTemplateRenderer>
        {
            
        }

        public class when_rendering_from_a_collection : Concern
        {
            Establish context = () =>
            {
                var data1 = new Collection.CollectionData
                            {
                                name = "foo name",
                                value = "foo value"
                            };
                var data2 = new Collection.CollectionData
                            {
                                name = "bar name",
                                value = "bar value"
                            };

                collection = new Collection
                {
                    href = "/blah",
                    template = new Collection.Template
                    {
                        data = new List<Collection.CollectionData>{data1, data2}
                    }
                };
                panelFactory = depends.on<ICreateInstances<StackPanel>>();
                textBoxFactory = depends.on<ICreateTextBox>();

                panel = fake.an<StackPanel>();
                panelFactory.Stub(x => x.GetInstance()).Return(panel);

                textBox1 = new TextBox {Text = data1.name};
                textBox2 = new TextBox {Text = data2.name };

                textBoxFactory.Stub(x => x.Get(data1.name)).Return(textBox1);
                textBoxFactory.Stub(x => x.Get(data2.name)).Return(textBox2);

                panelChildren = fake.an<StubUIElementCollection>();
                panel.Stub(x => x.Children).Return(panelChildren);
            };

            Because of = () =>
                result = sut.RenderFor(collection);

            It should_return_a_panel = () =>
                result.ShouldEqual(panel);

            It should_have_added_an_input_for_each_data_in_the_template = () =>
            {
                panelChildren.AssertWasCalled(x => x.Add(textBox1));
                panelChildren.AssertWasCalled(x => x.Add(textBox2));
//                panelChildren.AssertWasCalled(x => x.Add(Arg<UIElement>.Is.Anything));

            };
                

            private static StackPanel result;
            private static StackPanel panel;
            private static Collection collection;
            private static ICreateInstances<StackPanel> panelFactory;
            private static StubUIElementCollection panelChildren;
            private static TextBox textBox1;
            private static TextBox textBox2;
            private static ICreateTextBox textBoxFactory;

            public class StubUIElementCollection : UIElementCollection
            {
                public StubUIElementCollection() : base(new UIElement(), null)
                {
                }
            }
        }
         
    }
}