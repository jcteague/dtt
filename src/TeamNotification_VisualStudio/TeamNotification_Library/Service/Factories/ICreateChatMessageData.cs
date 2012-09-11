using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Factories
{
    public interface ICreateChatMessageModel
    {
        ChatMessageModel Get(string message);
    }
}