namespace TeamNotification_Library.Models
{
    public class CodeClipboardData : ChatMessageData
    {
        public string message { get; set; }

        public string solution { get; set; }
        
        public string document { get; set; }

        public int line { get; set; }
    }
}