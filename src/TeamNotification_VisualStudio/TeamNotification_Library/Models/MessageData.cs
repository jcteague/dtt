using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamNotification_Library.Models
{
    public class MessageData
    {
        public string body { get; set; }
        public string date { get; set; }
        public string name { get; set; }
        public string user_id { get; set; }
    }

    public class MessageBody : CodeClipboardData
    {
        
    }
}
