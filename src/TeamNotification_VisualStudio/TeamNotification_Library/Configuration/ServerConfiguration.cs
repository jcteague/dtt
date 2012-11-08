using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace TeamNotification_Library.Configuration
{
    public class ServerConfiguration : IStoreConfiguration
    {
        private string _uri = "";
        
        public string Uri
        {
            get { return Properties.Settings.Default.ApiSite; }
            set { _uri = value; }
        }
    }
}
