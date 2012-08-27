using System.Collections.Generic;
using System.Windows.Documents;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Chat.Formatters
{
    public interface IFormatPlainMessages
    {
        Maybe<Block> GetFormattedElement(ChatMessageModel chatMessage);
    }
}