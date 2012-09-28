using System.Windows;
using TeamNotification_Library.Extensions;

namespace TeamNotification_Library.Service.Clipboard
{
    public class SystemClipboardHandler : IHandleSystemClipboard
    {
        private string value;

        public string GetText(bool useInternal = false)
        {
            if (useInternal || value.IsNullOrEmpty())
            {
                if(System.Windows.Clipboard.ContainsText())
                {
                    return System.Windows.Clipboard.GetText();
                }
                return "";
            }
            return value;
        }

        public void SetText(string text)
        {
            value = text;
        }
    }
}