namespace TeamNotification_Library.Service.Http
{
    public interface ISendChatMessages
    {
        void SendMessage( string message, string roomId);
    }
}