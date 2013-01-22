Repository = require('./repository')
Q = require('q')
user_repository = new Repository('User')
chat_room_users_repository = new Repository('ChatRoomUser')
chat_room_repository = new Repository('ChatRoom')
redis_connector = require('./redis/redis_gateway')
redis_queryer = redis_connector.open()

update_messages_names = (user_id, new_first_name, new_last_name)->
    user_repository.find(id:user_id).then (users)->
        user = users[0]
        if(user.first_name != new_first_name || user.last_name != new_last_name)
            format_message = (original, room_messages_key, score)->
                deferred = Q.defer()
                redis_queryer.zrem room_messages_key, JSON.stringify(original), ()->
                    original.name = new_first_name
                    stringified = JSON.stringify(original)
                    redis_queryer.zadd room_messages_key, score, stringified, ()->
                        deferred.resolve()
                deferred.promise
                    
            change_room_messages = (chat_rooms)->
                handle_room_update = (j,l)->
                    if j >= l 
                        return
                    chat_room = chat_rooms[j]
                    room_messages_key = "room:#{chat_room.id}:messages"
                    console.log room_messages_key
                    callback = (err,messages)->
                        score = 0
                        loop_deferred = Q.defer()
                        do_loop = (i, count)->
                            if(i >= count)
                                loop_deferred.resolve()
                            else
                                original = JSON.parse(messages[i])
                                score = messages[i+1]
                                if original.user_id.toString() == user_id.toString()
                                    console.log messages[i]
                                    format_message(original,room_messages_key,score).then ()->
                                        do_loop(i+2, count)
                                else
                                    do_loop(i+2, count)
                                if( i == 0)
                                    loop_deferred.promise
                        if messages.length > 0
                            do_loop(0,messages.length).then ()->
                                handle_room_update(j+1,l)
                        else
                            handle_room_update(j+1,l)
                            
                    redis_queryer.zrevrange room_messages_key, 0, 100,'WITHSCORES', callback
                handle_room_update(0,chat_rooms.length)
                
            chat_room_users_repository.find(user_id:user_id).then (room_members)->
                chat_rooms_array = []
                if room_members?
                    console.log 'room_members'
                    for room_member in  room_members
                        chat_rooms_array.push room_member.chat_room #change_room_messages([room_member.chat_room])
                
                chat_room_repository.find(owner_id:user_id).then (chat_rooms)->
                    if chat_rooms?
                        chat_rooms_array.push.apply(chat_rooms_array, chat_rooms)
                    change_room_messages(chat_rooms)

module.exports =
    update_messages_names: update_messages_names
