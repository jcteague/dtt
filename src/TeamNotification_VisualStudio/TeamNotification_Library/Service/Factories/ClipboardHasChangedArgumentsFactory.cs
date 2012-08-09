using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Factories
{
    public class ClipboardHasChangedArgumentsFactory : ICreateClipboardArguments
    {
        public ClipboardHasChanged Get(string solution, string document, string text, int line)
        {
            return new ClipboardHasChanged
                       {
                           solution = solution,
                           document = document,
                           message = text,
                           line = line
                       };
        }
    }
}