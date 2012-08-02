using System.Collections.Generic;

namespace TeamNotification_Library.Models
{
    public class Collection
    {
        public string href { get; set; }
        public IEnumerable<Link> links { get; set; }
        public Template template { get; set; }
        public IEnumerable<Query> queries { get; set; }
        public IEnumerable<Room> rooms { get; set; }
        public IEnumerable<Members> members {get; set;}

        public class Link
        {
            public string name { get; set; }
            public string rel { get; set; }
            public string href { get; set; }
        }

        public class Members
        {
            public string href { get; set; }
            private IEnumerable<Link> data { get; set; }
        }

        public class Room
        {
            public string href { get; set; }
            public IEnumerable<Link> links { get; set; }
            public IEnumerable<CollectionData> data { get; set; }
        }


        public class Template
        {
            public IEnumerable<CollectionData> data { get; set; }
        }

        public class Query
        {
            public string href { get; set; }
            public string rel { get; set; }
            public string prompt { get; set; }
            public string type { get; set; }
            public string submit { get; set; }
        }
    }
}