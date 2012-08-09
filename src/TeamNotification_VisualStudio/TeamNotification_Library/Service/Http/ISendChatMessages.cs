using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Http
{
    public interface ISendChatMessages
    {
        void SendMessage(ChatMessageData message, string roomId);
    }
}