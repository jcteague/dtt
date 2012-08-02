using System.Windows;
using System.Windows.Controls;
using Machine.Specifications;
using Rhino.Mocks;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Content;
using TeamNotification_Library.Service.Factories.UI;
using developwithpassion.specifications.rhinomocks;

namespace TeamNotification_Test.Library.Service.Content
{
    [Subject(typeof(StackPanelBuilder))]
    public class StackPanelBuilderSpecs
    {
        public abstract class Concern : Observes<IBuildStackPanels, StackPanelBuilder>
        {
            Establish context = () =>
            {
                collectionData = new CollectionData { name = "foo", label = "foo label" };

                stackPanelFactory = depends.on<ICreateUIElements<StackPanel>>();
                passwordFactory = depends.on<ICreateUIElements<PasswordBox>>();
                textBoxFactory = depends.on<ICreateUIElements<TextBox>>();
                labelFactory = depends.on<ICreateLabels>();

                panel = fake.an<StackPanel>();
                stackPanelFactory.Stub(x => x.Get(collectionData.name + "Panel")).Return(panel);

                panelChildren = fake.an<UIElementCollectionStub>();
                panel.Stub(x => x.Children).Return(panelChildren);

                label = new Label { Content = collectionData.label };
                labelFactory.Stub(x => x.Get(collectionData.label)).Return(label);
            };

            protected static CollectionData collectionData;
            protected static StackPanel result;
            protected static StackPanel panel;
            protected static ICreateUIElements<StackPanel> stackPanelFactory;
            protected static ICreateUIElements<PasswordBox> passwordFactory;
            protected static ICreateUIElements<TextBox> textBoxFactory;
            protected static ICreateLabels labelFactory;
            protected static UIElementCollectionStub panelChildren;
            protected static Label label;
        }

        public class when_getting_a_stack_panel_from_a_field_and_the_field_is_a_password_box : Concern
        {
            Establish context = () =>
            {
                collectionData.type = Globals.Fields.Password;
                
                passwordBox = new PasswordBox {Name = collectionData.name};
                passwordFactory.Stub(x => x.Get(collectionData.name)).Return(passwordBox);
            };

            Because of = () =>
                result = sut.GetFor(collectionData);

            It should_return_instance_of_a_ui_element_for_the_specific_type = () =>
                result.ShouldEqual(panel);

            It should_add_a_label_for_the_collection_data = () =>
                panelChildren.AssertWasCalled(x => x.Add(label));

            It should_add_a_password_box_for_the_collection_data = () =>
                panelChildren.AssertWasCalled(x => x.Add(passwordBox));

            private static PasswordBox passwordBox;
        }

        public class when_getting_a_stack_panel_from_a_field_and_the_field_is_a_textbox : Concern
        {
            Establish context = () =>
            {
                collectionData.type = Globals.Fields.TextBox;
                
                textBox = new TextBox { Name = collectionData.name };
                textBoxFactory.Stub(x => x.Get(collectionData.name)).Return(textBox);
            };

            Because of = () =>
                result = sut.GetFor(collectionData);

            It should_return_instance_of_a_ui_element_for_the_specific_type = () =>
                result.ShouldEqual(panel);

            It should_add_a_label_for_the_collection_data = () =>
                panelChildren.AssertWasCalled(x => x.Add(label));

            It should_add_a_password_box_for_the_collection_data = () =>
                panelChildren.AssertWasCalled(x => x.Add(textBox));

            private static TextBox textBox;
        }

        public class UIElementCollectionStub : UIElementCollection
        {
            public UIElementCollectionStub()
                : base(new UIElement(), new FrameworkElement())
            {
            }
        }
    }
}