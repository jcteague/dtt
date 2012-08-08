namespace TeamNotification_Library.Service.Async.Models
{
    public class ClipboardHasChanged : IHaveEventArguments
    {
        public string Solution { get; set; }

        public string Document { get; set; }

        public string Text { get; set; }

        public int Line { get; set; }
    }
}