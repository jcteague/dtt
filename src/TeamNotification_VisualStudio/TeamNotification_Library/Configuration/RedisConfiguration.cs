using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamNotification_Library.Configuration
{
    public class RedisConfiguration : IStoreConfiguration
    {
        private string _href = "";
        public string Uri
        {
            get { return Properties.Settings.Default.redis; }
            set { _href = value; }
        }
    }
}
