using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeamNotification_Library.Extensions;

namespace TeamNotification_Library.Models
{
    public class MessageData
    {
        public string body { get; set; }
        public string stamp { get; set; }
        public string date { get; set; }
        public string name { get; set; }
        public string user_id { get; set; }

        public int GetUserId()
        {
            return user_id.ParseToInteger();
        }
    }
}
