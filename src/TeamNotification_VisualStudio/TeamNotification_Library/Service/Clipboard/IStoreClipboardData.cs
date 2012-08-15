using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Clipboard
{
    public interface IStoreClipboardData
    {
        void Store<T>(T clipboardArgs) where T : ChatMessageData;
        T Get<T>() where T : ChatMessageData, new();
        bool IsCode { get; }
    }
}