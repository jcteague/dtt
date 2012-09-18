using System.Windows;
using System.Windows.Controls;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Factories.UI;

namespace TeamNotification_Library.Service.Content
{
    public class StackPanelBuilder : IBuildStackPanels
    {
        private readonly ICreateUIElements<StackPanel> panelFactory;
        private readonly ICreateUIElements<PasswordBox> passwordBoxFactory;
        private readonly ICreateUIElements<TextBox> textBoxFactory;
        private readonly ICreateLabels labelFactory;

        public StackPanelBuilder(ICreateUIElements<StackPanel> panelFactory, ICreateUIElements<PasswordBox> passwordBoxFactory, ICreateUIElements<TextBox> textBoxFactory, ICreateLabels labelFactory)
        {
            this.panelFactory = panelFactory;
            this.passwordBoxFactory = passwordBoxFactory;
            this.textBoxFactory = textBoxFactory;
            this.labelFactory = labelFactory;
        }

        public StackPanel GetFor(CollectionData collectionData)
        {
            var panel = panelFactory.Get(collectionData.name + "Panel");
            panel.DataContext = collectionData;
            panel.Orientation = Orientation.Horizontal;

            var children = panel.Children;
            children.Add(labelFactory.Get(collectionData.label));

            UIElement element;
            if (collectionData.type == GlobalConstants.Fields.Password)
                element = passwordBoxFactory.Get(collectionData.name);
            else
                element = textBoxFactory.Get(collectionData.name);

            children.Add(element);
            return panel;
        }
    }
}