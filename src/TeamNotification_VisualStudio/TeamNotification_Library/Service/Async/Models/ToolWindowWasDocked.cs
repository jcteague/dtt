namespace TeamNotification_Library.Service.Async.Models
{
    public class ToolWindowWasDocked : IHaveEventArguments
    {
        public bool isDocked { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int w { get; set; }
        public int h { get; set; }
    }
}