namespace TeamNotification_Library.Service.System
{
    public class ApplicationGlobalState : IStoreGlobalState
    {
        public bool Active { get; set; }
    }
}