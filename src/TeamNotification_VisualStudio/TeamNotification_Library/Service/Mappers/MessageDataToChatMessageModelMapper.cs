using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Extensions;

namespace TeamNotification_Library.Service.Mappers
{
    public class MessageDataToChatMessageModelMapper : IMapEntities<MessageData, ChatMessageModel>
    {
        private ISerializeJSON jsonSerializer;

        public MessageDataToChatMessageModelMapper(ISerializeJSON jsonSerializer)
        {
            this.jsonSerializer = jsonSerializer;
        }

        public ChatMessageModel MapFrom(MessageData source)
        {
            var messageBody = jsonSerializer.Deserialize<MessageBody>(source.body);
            return new ChatMessageModel
                       {
                           UserId = source.GetUserId(),
                           UserName = source.name,
                           Message = messageBody.message,
                           DateTime = source.date.ToDateTime(),
                           Document = messageBody.document,
                           Column = messageBody.column,
                           Line = messageBody.line,
                           Solution = messageBody.solution,
                           ProgrammingLanguage = messageBody.programmingLanguage
                       };

        }
    }
}