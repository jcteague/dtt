using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamNotification_Library.Configuration
{
    public class RedisConfiguration : IStoreConfiguration
    {
        private string _href = "dtt.local:6379";
        public string Uri
        {
            get { return _href; }
            set { _href = value; }
        }
    }
}
