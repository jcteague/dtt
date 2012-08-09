using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Clipboard
{
    public interface IStoreClipboardData
    {
        ClipboardHasChanged Data { get; }
//        ClipboardHasChanged ToSend { get; set; }
        void Store(ClipboardHasChanged clipboardArgs);
    }
}