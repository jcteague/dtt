using System.Windows.Interop;

namespace TeamNotification_Library.Service.Clipboard
{
    public interface IHandleVisualStudioClipboard
    {
        void SetUpFor(HwndSource hwndSource);
    }
}