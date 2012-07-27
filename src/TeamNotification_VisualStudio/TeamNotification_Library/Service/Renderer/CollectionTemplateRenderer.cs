using System.Windows.Controls;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Factories;
using TeamNotification_Library.Service.Factories.UI;

namespace TeamNotification_Library.Service.Renderer
{
    public class CollectionTemplateRenderer : IRenderCollectionTemplate
    {
        private ICreateInstances<StackPanel> panelFactory;
        private TextBoxFactory textBoxFactory;

        public CollectionTemplateRenderer(ICreateInstances<StackPanel> panelFactory, TextBoxFactory textBoxFactory)
        {
            this.panelFactory = panelFactory;
            this.textBoxFactory = textBoxFactory;
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
                panel.Children.Add(label);
                panel.Children.Add(textBox);
            }
            return panel;
        }
    }
}