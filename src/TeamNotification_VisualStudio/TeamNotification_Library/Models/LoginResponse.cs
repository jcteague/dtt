using System;
using System.Collections.Generic;
using System.Linq;

namespace TeamNotification_Library.Models
{
    public class LoginResponse
    {
        public bool success { get; set; }
        public Collection.RedisConfig redis { get; set; }
        public User user { get; set; }

        public IEnumerable<string> SerializeForFile()
        {
            List<string> values = this.user.SerializeForFile().ToList();
            values.AddRange(this.redis.GetType().GetProperties().Select(prop => Convert.ToString(prop.GetValue(this.redis, null))));
            return values; //GetType().GetProperties().Select(x => x.GetValue(this, null)).Where(x => x != null).Select(x => x.ToString());
        }
    }
}