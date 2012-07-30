using System.Windows;
using System.Windows.Controls;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Factories;
using TeamNotification_Library.Service.Factories.UI;

namespace TeamNotification_Library.Service.Renderer
{
    public class CollectionTemplateRenderer : IRenderCollectionTemplate
    {
        private ICreateInstances<StackPanel> panelFactory;
        private ICreateTextBox textBoxFactory;
        private ICreateButtons buttonFactory;

        public CollectionTemplateRenderer(ICreateInstances<StackPanel> panelFactory, ICreateTextBox textBoxFactory, ICreateButtons buttonFactory)
        {
            this.panelFactory = panelFactory;
            this.textBoxFactory = textBoxFactory;
            this.buttonFactory = buttonFactory;
        }

        public StackPanel RenderFor(Collection collection)
        {
            var panel = panelFactory.GetInstance();
            foreach(var data in collection.template.data)
            {
                var textBox = textBoxFactory.Get(data.name);
                var label = new TextBlock
                                {
                                    Text = data.label
                                };
//                panel.Children.Add(label);
                panel.Children.Add(textBox);
//                var button = buttonFactory.Get();
//                panel.Children.Add(button);
            }
            return panel;
        }
    }
}