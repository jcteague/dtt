using System.Windows;

namespace TeamNotification_Library.Service.Clipboard
{
    public class SystemClipboardHandler : IHandleSystemClipboard
    {
        private string value;

        public string GetText(bool useInternal = false)
        {
            if (useInternal)
                return System.Windows.Clipboard.GetText();
            return value;
        }

        public void SetText(string text)
        {
            value = text;
        }
    }
}