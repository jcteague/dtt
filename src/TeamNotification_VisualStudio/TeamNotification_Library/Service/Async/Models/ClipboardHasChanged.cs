namespace TeamNotification_Library.Service.Async.Models
{
    public class ClipboardHasChanged : IHaveEventArguments
    {
        public string solution { get; set; }

        public string document { get; set; }

        public string message { get; set; }

        public int line { get; set; }
    }
}