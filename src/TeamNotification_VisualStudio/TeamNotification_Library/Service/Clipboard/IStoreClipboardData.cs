using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Clipboard
{
    public interface IStoreClipboardData
    {
        void Store<T>(T clipboardArgs) where T : ChatMessageModel;
        T Get<T>() where T : ChatMessageModel, new();
    }
}