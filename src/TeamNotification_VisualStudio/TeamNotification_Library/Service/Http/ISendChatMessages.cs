using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Http
{
    public interface ISendChatMessages
    {
        void SendMessage<T>(T message, string roomId) where T : ChatMessageData;
    }
}