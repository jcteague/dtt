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

        public LoginControl(ISendHttpRequests httpClient)
        {
            InitializeComponent();
//            var collection = new List<CollectionData>
//                           {
//                               new CollectionData {label = "First Element"},
//                               new CollectionData {label = "Second Element"}
//                           };
            var collection = httpClient.Get<Collection>(href).Result;
//            this.Resources.Add("collection", collection);
            this.Resources.Add("templateData", collection.template.data);
        }
    }

    public class CollectionData
    {
        public string label { get; set; }
    }
}
