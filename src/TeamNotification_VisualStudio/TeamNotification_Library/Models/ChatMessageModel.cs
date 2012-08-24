using System;
using TeamNotification_Library.Extensions;

namespace TeamNotification_Library.Models
{
    public class ChatMessageModel
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Message { get; set; }

        public DateTime DateTime { get; set; }

        public string Solution { get; set; }

        public string Document { get; set; }

        public int Line { get; set; }

        public int Column { get; set; }
        
        public int ProgrammingLanguage { get; set; }
        
        public bool IsCode()
        {
            return !Solution.IsNullOrEmpty();
        }
    }
}