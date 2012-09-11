using System;
using System.Windows;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Service.Http;

namespace TeamNotification_Library.Models
{
    public class ChatMessageModel
    {
        private ISerializeJSON jsonSerializer;
        public ChatMessageModel()
        {
            stamp = name = user_id = username = date = ""; 
            //message = project = solution = document = ""; line = column = programminglanguage = -1;
            jsonSerializer = new JSONSerializer();
        }

        public string stamp { get; set; }
        
        public string body { get; set; }

        public string name { get; set; }

        public string user_id { get; set; }

        public string username
        {
            get { return name; }
            set { name = value; }
        }

        public string date { get; set; }

        public ChatMessageBody chatMessageBody;

        //public string message { get; set; }
        
        //public string project { get; set; }

        //public string solution { get; set; }

        //public string document { get; set; }

        //public int line { get; set; }

        //public int column { get; set; }
        
        //public int programminglanguage { get; set; }
        
        //public bool IsCode
        //{
        //    get { return !solution.IsNullOrEmpty(); }
        //}
        public int GetUserId()
        {
            return user_id.ParseToInteger();
        }
        public ResourceDictionary AsResources()
        {
            var resources = new ResourceDictionary();
            resources["solution"] = chatMessageBody.solution;
            resources["project"] = chatMessageBody.project;
            resources["document"] = chatMessageBody.document;
            resources["line"] = chatMessageBody.line;
            resources["column"] = chatMessageBody.column;
            resources["message"] = chatMessageBody.message;
            resources["programminglanguage"] = chatMessageBody.programminglanguage;
            resources["stamp"] = stamp;
            resources["date"] = date;
            return resources;
        }

        public void FillChatMessageBody()
        {
            chatMessageBody = jsonSerializer.Deserialize<ChatMessageBody>(body);
        }
    }
}