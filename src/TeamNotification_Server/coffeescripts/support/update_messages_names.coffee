Repository = require('./repository')
user_repository = new Repository('User')
chat_room_users_repository = new Repository('ChatRoomUser')
chat_room_repository = new Repository('ChatRoom')
redis_connector = require('../support/redis/redis_gateway')
redis_queryer = redis_connector.open()

update_messages_names = (user_id, new_first_name, new_last_name)->
    user_repository.find(id:user_id).then (users)->        
        user = users[0]
        if(user.first_name != new_first_name || user.last_name != new_last_name)
            format_message = (original, room_messages_key, score)->
                if original.user_id.toString() == user_id.toString()
                    original.name = new_first_name
                    console.log 'original'
                    console.log original
                    stringified = JSON.stringify(original)
                    console.log 'stringified'
                    console.log stringified
                    redis_queryer.zremrangebyscore room_messages_key, score, score
                    redis_queryer.zadd room_messages_key, score, stringified
            
            change_room_messages = (chat_rooms)->
                console.log chat_rooms
                for chat_room in  chat_rooms
                    room_messages_key = "room:#{chat_room.id}:messages"
                    callback = (err,messages)->
                        i = 0
                        score = 0
                        for message in messages
                            if (i&1) == 0
                                original = JSON.parse(message)
                            else
                                score = message
                                format_message(original,room_messages_key,score)
                            i=i+1
                    redis_queryer.zrevrange room_messages_key, 0, 100,'WITHSCORES', callback

            chat_room_users_repository.find(user_id:user_id).then (room_members)->
                if room_members?
                    for room_member in  room_members
                        change_room_messages([room_member.chat_room])
                
            chat_room_repository.find(owner_id:user_id).then (chat_rooms)->
                if chat_rooms?
                    change_room_messages(chat_rooms)

module.exports =
    update_messages_names: update_messages_names
