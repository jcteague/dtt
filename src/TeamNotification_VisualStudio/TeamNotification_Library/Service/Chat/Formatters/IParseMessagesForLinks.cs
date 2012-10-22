using System.Windows.Documents;

namespace TeamNotification_Library.Service.Chat.Formatters
{
    public interface IParseMessagesForLinks
    {
        Span Parse(string message);
    }
}