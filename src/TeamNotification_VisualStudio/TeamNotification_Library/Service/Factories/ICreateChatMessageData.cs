using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Factories
{
    public interface ICreateChatMessageData
    {
        ChatMessageData Get(string message);
    }
}