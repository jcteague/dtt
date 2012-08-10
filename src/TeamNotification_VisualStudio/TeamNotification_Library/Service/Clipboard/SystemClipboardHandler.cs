using System.Windows;

namespace TeamNotification_Library.Service.Clipboard
{
    public class SystemClipboardHandler : IHandleSystemClipboard
    {
        public string GetText()
        {
            return System.Windows.Clipboard.GetText();
        }

        public void SetText(string text)
        {
            System.Windows.Clipboard.SetData(DataFormats.Text, text);
        }
    }
}