using System.Collections.Generic;

namespace TeamNotification_Library.Models
{
    public class Collection
    {
        public string href { get; set; }
        public IEnumerable<Link> links { get; set; }
        public Template template { get; set; }
        public IEnumerable<Query> queries { get; set; }

        public class Link
        {
            public string name { get; set; }
            public string rel { get; set; }
            public string href { get; set; }
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

        public class CollectionData
        {
            public string name { get; set; }
            public string value { get; set; }
            public string label { get; set; }
            public string type { get; set; }
            public string maxlength { get; set; }
        }
    }
}