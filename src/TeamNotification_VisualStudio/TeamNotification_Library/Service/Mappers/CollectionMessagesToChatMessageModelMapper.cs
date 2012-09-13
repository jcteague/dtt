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
            var messageBody = jsonSerializer.Deserialize<ChatMessageBody>(Collection.getField(source.data, "body"));
            var username = Collection.getField(source.data, "user");
            var userId = Collection.getField(source.data, "user_id");
            var dateTime = Collection.getField(source.data, "datetime");
            var stamp = Collection.getField(source.data, "stamp");
            return new ChatMessageModel
                       {
                           user_id = userId,
                           username = username,
                           date = dateTime,
                           stamp = stamp,
                           chatMessageBody = new ChatMessageBody
                           {
                               message = messageBody.message,
                               project = messageBody.project,
                               solution = messageBody.solution,
                               document = messageBody.document,
                               line = messageBody.line,
                               column = messageBody.column,
                               programminglanguage = messageBody.programminglanguage,
                               date = dateTime,
                               stamp = stamp
                           }
                       };
        }
    }
}