using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamNotification_Library.Configuration
{
    public class ServerConfiguration : IStoreConfiguration
    {
        public string Uri
        {
            get { return "http://dtt.local:3000/"; }
            set { Uri = value; }
        }
    }
}
