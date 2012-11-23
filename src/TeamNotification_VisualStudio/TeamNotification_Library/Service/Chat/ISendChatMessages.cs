using System.Collections.Generic;
using System.Windows.Documents;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Chat
{
    public interface ISendChatMessages
    {
        void Initialize();

        void SendMessage(ChatMessageBody editedMessage, string roomId);
        //void SendMessages(IEnumerable<Block> blocks, string roomId);
        void SendMessages(IEnumerable<ChatMessageBody> messages, string roomId);
    }
}