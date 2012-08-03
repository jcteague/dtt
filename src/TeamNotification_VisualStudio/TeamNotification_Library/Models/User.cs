using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TeamNotification_Library.Configuration;

namespace TeamNotification_Library.Models
{
    public class User
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public IEnumerable<string> SerializeForFile()
        {
            return GetType().GetProperties().Select(x => x.GetValue(this, null)).Where(x => x != null).Select(x => x.ToString());
        }
    }
}