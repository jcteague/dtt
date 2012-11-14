using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.Async.Models
{
    public class CodeWasAppended
    {
        public ChatMessageModel ChatMessageModel { get; private set; }

        public CodeWasAppended(ChatMessageModel chatMessageModel)
        {
            ChatMessageModel = chatMessageModel;
        }
    }
}