using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamNotification_Library.Configuration
{
    public class ServerConfiguration : IStoreConfiguration
    {
        private string _uri = "http://dtt.local:3000/";
        
        public string Uri
        {
            get { return _uri; }
            set { _uri = value; }
        }
    }
}
