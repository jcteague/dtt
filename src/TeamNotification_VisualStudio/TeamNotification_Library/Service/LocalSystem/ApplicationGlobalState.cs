namespace TeamNotification_Library.Service.LocalSystem
{
    public class ApplicationGlobalState : IStoreGlobalState
    {
        public bool Active { get; set; }
        
        public bool IsEditingCode { get; set; }
    }
}