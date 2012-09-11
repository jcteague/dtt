using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Factories
{
    public class ChatMessageModelFactory : ICreateChatMessageModel
    {
        public ChatMessageModel Get(string message)
        {
            return new ChatMessageModel
                       {
                           chatMessageBody = new ChatMessageBody { message = message }
                       };
        }
    }
}