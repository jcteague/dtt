using System;
using System.Collections.Generic;
using System.Linq;
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
using TeamNotification_Library.Service.Http;

namespace AvenidaSoftware.TeamNotification_Package.Controls
{
    /// <summary>
    /// Interaction logic for LoginControl.xaml
    /// </summary>
    public partial class LoginControl : UserControl
    {
        private string href = "http://dtt.local:3000/registration?&userName=Raymi&userMessage=hellothere";

        private ISendHttpRequests httpClient;

        public LoginControl(ISendHttpRequests httpClient)
        {
            this.httpClient = httpClient;
            InitializeComponent();

            var collection = httpClient.Get<Collection>(href).Result;
            Resources.Add("templateData", collection.template.data);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (CollectionData item in listBox1.Items)
            {
                System.Diagnostics.Debug.WriteLine(item);
            }
        }
    }
}
