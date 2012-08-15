using System.Windows.Documents;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Http
{
    public interface ISendChatMessages
    {
        void SendMessage(Block message, string roomId);
    }
}