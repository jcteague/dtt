using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.Controls;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.Providers;

namespace AvenidaSoftware.TeamNotification_Package.Controls
{
    /// <summary>
    /// Interaction logic for LoginControl.xaml
    /// </summary>
    public partial class LoginControl : UserControl
    {
        private IServiceLoginControl loginControlService;
        private IProvideUser userProvider;

        public LoginControl(IServiceLoginControl loginControlService, IProvideUser userProvider)
        {
            this.loginControlService = loginControlService;
            this.userProvider = userProvider;

            InitializeComponent();
            
            var collection = loginControlService.GetCollection();
            Resources.Add("templateData", collection.template.data);

            loginControlService.UserHasLogged += (sender, e) => this.Content = Container.GetInstance<MyControl>();
            loginControlService.UserCouldNotLogIn += (sender, e) => MessageBox.Show("User and passwords are incorrect");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var collection = new List<CollectionData>();
            foreach (CollectionData item in templateContainer.Items)
            {
                collection.Add(item);
            }
            loginControlService.HandleClick(collection);
        }

        private IEnumerable<StackPanel> GetTemplateFrom(Collection collection)
        {
            var panels = new List<StackPanel>();
            foreach (var field in collection.template.data)
            {
                var stackPanel = new StackPanel();
                var label = new Label {Content = field.label};
                stackPanel.Children.Add(label);

                if(field.type == "password")
                {
                    var passwordBox = new PasswordBox();
//                    passwordBox.SetBinding(PasswordBox.)
                    stackPanel.Children.Add(passwordBox);
                }
                else
                {
                    var textbox = new TextBox();
                    textbox.SetBinding(TextBox.TextProperty, field.value);
                    stackPanel.Children.Add(textbox);
                }
            }
            return panels;
        }
    }
}
