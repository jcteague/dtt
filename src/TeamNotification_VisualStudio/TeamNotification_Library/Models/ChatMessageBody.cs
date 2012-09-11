using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Extensions;

namespace TeamNotification_Library.Models
{
    public class ChatMessageBody
    {
        public ChatMessageBody()
        {
            project = solution = document = stamp = date ="";
            line = column = programminglanguage = -1;
        }

        private string _project;
        private string _solution;
        private string _document;

        public string date { get; set; }

        public string stamp { get; set; }

        public string message { get; set; }

        public string project 
        { 
            get { return _project; }
            set { _project = StripDirectories(value); }
        }

        public string solution
        {
            get { return _solution; }
            set { _solution = StripDirectories(value); }
        }

        public string document
        {
            get { return _document; }
            set { _document = StripDirectories(value); }
        }

        public int line { get; set; }

        public int column { get; set; }

        public int programminglanguage { get; set; }
        
        public bool IsCode
        {
            get { return !solution.IsNullOrEmpty(); }
        }
        private string StripDirectories(string path)
        {
            var shr = "";
            for (var i = path.Length - 1; i >= 0 && path[i] != '\\'; --i)
                shr = path[i] + shr;
            return shr;
        }
    }
}
