using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Extensions;

namespace TeamNotification_Library.Service.Mappers
{
    public class MessageDataToChatMessageModelMapper : IMapEntities<Collection.Messages, ChatMessageModel>
    {
        private ISerializeJSON jsonSerializer;

        public MessageDataToChatMessageModelMapper(ISerializeJSON jsonSerializer)
        {
            this.jsonSerializer = jsonSerializer;
        }

        public ChatMessageModel MapFrom(MessageData source)
        {
            var messageBody = jsonSerializer.Deserialize<ChatMessageData>(source.body);
            return new ChatMessageModel
                       {
                           user_id = source.GetUserId(),
                           username = source.name,
                           message = messageBody.message,
                           datetime = source.date.ToDateTime(),
                           project = messageBody.project,
                           document = messageBody.document,
                           column = messageBody.column,
                           line = messageBody.line,
                           solution = messageBody.solution,
                           programminglanguage = messageBody.programmingLanguage
                       };

        }
    }
}