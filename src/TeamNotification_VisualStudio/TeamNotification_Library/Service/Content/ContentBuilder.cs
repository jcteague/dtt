using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Content
{
    public class ContentBuilder : IBuildContent
    {
        public IEnumerable<StackPanel> GetContentFor(Collection collection)
        {
            var panels = new List<StackPanel>();
            foreach (var field in collection.template.data)
            {
                var stackPanel = new StackPanel { DataContext = field, Orientation = Orientation.Horizontal };
                var label = new Label { Content = field.label, Width = 75 };
                stackPanel.Children.Add(label);
                if (field.type == "password")
                {
                    var passwordBox = new PasswordBox { Name = field.name, Width = 100 };
                    stackPanel.Children.Add(passwordBox);
                }
                else
                {
                    var textbox = new TextBox { Name = field.name, Width = 100 };
                    textbox.SetBinding(TextBox.TextProperty, new Binding("value"));
                    stackPanel.Children.Add(textbox);
                }
                panels.Add(stackPanel);
            }
            return panels;
        }
    }
}