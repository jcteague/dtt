using TeamNotification_Library.Extensions;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Http;

namespace TeamNotification_Library.Service.Mappers
{
    public class CollectionMessagesToChatMessageModelMapper : IMapEntities<Collection.Messages, ChatMessageModel>
    {
        private ISerializeJSON jsonSerializer;

        public CollectionMessagesToChatMessageModelMapper(ISerializeJSON jsonSerializer)
        {
            this.jsonSerializer = jsonSerializer;
        }

        public ChatMessageModel MapFrom(Collection.Messages source)
        {
            var messageBody = jsonSerializer.Deserialize<MessageBody>(Collection.getField(source.data, "body"));
            var username = Collection.getField(source.data, "user");
            var userId = Collection.getField(source.data, "user_id");
            var dateTime = Collection.getField(source.data, "datetime").ToDateTime();

            return new ChatMessageModel
                       {
                           UserId = userId.ParseToInteger(),
                           UserName = username,
                           Message = messageBody.message,
                           DateTime = dateTime,
                           Project = messageBody.project,
                           Solution = messageBody.solution,
                           Document = messageBody.document,
                           Line = messageBody.line,
                           Column = messageBody.column,
                           ProgrammingLanguage = messageBody.programmingLanguage
                       };
        }
    }
}