﻿using System;
using System.Collections.Generic;
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
using TeamNotification_Library.Service.Controls;
using TeamNotification_Library.Service.Http;

namespace AvenidaSoftware.TeamNotification_Package.Controls
{
    /// <summary>
    /// Interaction logic for LoginControl.xaml
    /// </summary>
    public partial class LoginControl : UserControl
    {
        // TODO: This href should not be hardcoded here. How should it be passed it to the control?
        private string href = "http://dtt.local:3000/user/login";
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
            loginControlService.HandleClick();
            
//            var data = new List<KeyValuePair<string, string>>();
//            foreach (CollectionData item in listBox1.Items)
//            {
//                data.Add(new KeyValuePair<string, string>(item.name, item.value));
//            }
//
//            var content = new FormUrlEncodedContent(data);
//            var result = httpClient.Post<LoginResponse>(href, content).Result;
//            System.Diagnostics.Debug.WriteLine(result);
        }
    }
}
