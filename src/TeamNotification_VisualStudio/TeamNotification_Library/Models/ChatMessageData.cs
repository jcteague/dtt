using System;
using System.Windows;

namespace TeamNotification_Library.Models
{
    public class ChatMessageData
    {
        public ChatMessageData()
        {
            stamp = "";
        }

        public string message { get; set; }
        
        public string stamp { get; set; }        
        
        public string project { get; set; }

        public string solution { get; set; }

        public string document { get; set; }

        public int line { get; set; }

        public int column { get; set; }

        public int programmingLanguage { get; set; }

        private void ClearFields()
        {
            var shr = "";
            for (var i = project.Length - 1; i >= 0 && project[i] != '\\'; --i)
                shr = project[i] + shr;
            project = shr;
            shr = "";
            for (var i = solution.Length - 1; i >= 0 && solution[i] != '\\'; --i)
                shr = solution[i] + shr;
            solution = shr;
            shr = "";
            for (var i = document.Length - 1; i >= 0 && document[i] != '\\'; --i)
                shr = document[i] + shr;
            document = shr;
        }
        public ResourceDictionary AsResources()
        {
            ClearFields();
            var resources = new ResourceDictionary();
            resources["solution"] = solution;
            resources["project"] = project;
            resources["document"] = document;
            resources["line"] = line;
            resources["message"] = message;
            resources["column"] = column;
            resources["programmingLanguage"] = programmingLanguage;

            return resources;
        }
        public bool IsCode
        {
            get { return !String.IsNullOrEmpty(solution); }
        }
    }
}