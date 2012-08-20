using System.Windows;

namespace TeamNotification_Library.Models
{
    public class CodeClipboardData : ChatMessageData, ICanBeMappedAsResources
    {
        public string message { get; set; }

        public string solution { get; set; }
        
        public string document { get; set; }

        public int line { get; set; }
        
        public int column { get; set; }

        public ResourceDictionary AsResources()
        {
            var resources = new ResourceDictionary();
            resources["solution"] = solution;
            resources["document"] = document;
            resources["line"] = line;
            resources["message"] = message;
            resources["column"] = column;

            return resources;
        }
    }
}