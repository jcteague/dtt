using System.Collections.Generic;
using System.Windows.Documents;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Http
{
    public interface ISendChatMessages
    {
        void SendMessage(ChatMessageBody editedMessage, string roomId);
        void SendMessages(IEnumerable<Block> blocks, string roomId);
    }
}