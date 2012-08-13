namespace TeamNotification_Library.Service.Clipboard
{
    public interface IHandleSystemClipboard
    {
        string GetText(bool useInternal = false);
        void SetText(string text);
    }
}