using System.Collections.Generic;
using System.Windows.Documents;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Http
{
    public interface ISendChatMessages
    {
        void SendMessages(IEnumerable<Block> blocks, string roomId);

        void SendMessage(Block message, string roomId);
    }
}