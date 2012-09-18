using System;
using System.Windows;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Service.Http;

namespace TeamNotification_Library.Models
{
    public class ChatMessageModel
    {
        private ISerializeJSON jsonSerializer;
        private string _body = "";
        public ChatMessageModel()
        {
            name = user_id = username = ""; 
            jsonSerializer = new JSONSerializer();
        }

        public string stamp
        {
            get { return chatMessageBody.stamp; }
            set { if(_chatMessageBody!=null)
                    _chatMessageBody.stamp = value;
            }
        }

        public string body
        {
            get { return _body; }
            set {
                _body = value;
                if(_chatMessageBody == null)
                    _chatMessageBody = jsonSerializer.Deserialize<ChatMessageBody>(value);
            } 
        }

        public string name { get; set; }

        public string user_id { get; set; }

        public string username
        {
            get { return name; }
            set { name = value; }
        }

        public string date
        {
            get { return   chatMessageBody.date; }
            set { if (_chatMessageBody != null) _chatMessageBody.date = value; }
        }

        public virtual ChatMessageBody chatMessageBody
        {
            get { return _chatMessageBody ?? (_chatMessageBody = (_body==null)?null:jsonSerializer.Deserialize<ChatMessageBody>(_body)); }
            set { _chatMessageBody = value;
                _body = jsonSerializer.Serialize(value);
            }
        }

        private ChatMessageBody _chatMessageBody;
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
            var tmpChatMessageBody = this.chatMessageBody;
            resources["solution"] = tmpChatMessageBody.solution;
            resources["project"] = tmpChatMessageBody.project;
            resources["document"] = tmpChatMessageBody.document;
            resources["line"] = tmpChatMessageBody.line;
            resources["column"] = tmpChatMessageBody.column;
            resources["message"] = tmpChatMessageBody.message;
            resources["programminglanguage"] = tmpChatMessageBody.programminglanguage;
            resources["stamp"] = stamp;
            resources["date"] = date;
            return resources;
        }
    }
}