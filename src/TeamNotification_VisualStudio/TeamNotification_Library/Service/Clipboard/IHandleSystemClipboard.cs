namespace TeamNotification_Library.Service.Clipboard
{
    public interface IHandleSystemClipboard
    {
        string GetText();
        void SetText(string text);
    }
}