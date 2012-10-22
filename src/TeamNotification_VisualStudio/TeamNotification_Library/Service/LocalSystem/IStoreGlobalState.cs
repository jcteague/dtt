namespace TeamNotification_Library.Service.LocalSystem
{
    public interface IStoreGlobalState
    {
        bool Active { get; set; }

        bool IsEditingCode { get; set; }

    }
}