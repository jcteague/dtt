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

        public LoginControl(IServiceLoginControl loginControlService)
        {
            this.loginControlService = loginControlService;

            InitializeComponent();
            
            var collection = loginControlService.GetCollection();
            Resources.Add("templateData", collection.template.data);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(loginControlService.IsUserLogged())
            {
                Debug.WriteLine("User is logged in");
            }

            var collection = new List<CollectionData>();
            foreach (CollectionData item in listBox1.Items)
            {
                collection.Add(item);
            }
            loginControlService.HandleClick(collection);
        }
    }
}
