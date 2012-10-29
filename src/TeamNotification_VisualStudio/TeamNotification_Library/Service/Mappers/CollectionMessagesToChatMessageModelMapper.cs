using TeamNotification_Library.Extensions;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Http;

namespace TeamNotification_Library.Service.Mappers
{
    public class CollectionMessagesToChatMessageModelMapper : IMapEntities<Collection.Messages, ChatMessageModel>
    {
        public ChatMessageModel MapFrom(Collection.Messages source)
        {
            var username = Collection.getField(source.data, "user");
            var userId = Collection.getField(source.data, "user_id");
            return new ChatMessageModel
                       {
                           body = Collection.getField(source.data, "body"),
                           user_id = userId,
                           username = username
                       };
        }
    }
}