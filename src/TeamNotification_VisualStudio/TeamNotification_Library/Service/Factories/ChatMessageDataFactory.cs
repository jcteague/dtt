using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Factories
{
    public class ChatMessageDataFactory : ICreateChatMessageData
    {
        public ChatMessageData Get(string message)
        {
            return new ChatMessageData
                       {
                           message = message
                       };
        }
    }
}