using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Factories
{
    public interface ICreateClipboardArguments
    {
        ClipboardHasChanged Get(string solution, string document, string text, int line);
    }
}