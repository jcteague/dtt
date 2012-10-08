using System.Windows;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Service.Logging;

namespace TeamNotification_Library.Service.Clipboard
{
    public class SystemClipboardHandler : IHandleSystemClipboard
    {
        private ILog logger;

        private string value;

        public SystemClipboardHandler(ILog logger)
        {
            this.logger = logger;
        }

        public string GetText(bool useInternal = false)
        {
            logger.Info("Clipboard has {0} on value".FormatUsing(value));
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