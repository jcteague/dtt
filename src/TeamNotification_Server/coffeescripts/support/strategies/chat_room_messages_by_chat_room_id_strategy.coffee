Repository = require('../repository')
redis = require('../redis/redis_gateway').open()
Q = require('q')

chat_rooms = require('../strategies/chat_rooms_by_owner_id_or_member_strategy')
chat_room_members = require('../strategies/chat_room_by_id_strategy')

message = null
create_callback = (deferred, room_id, user) ->
    return (err, reply) ->
        if !err
            rooms = []
            members = []
            chat_rooms(user.id).then (user_and_rooms)->
                for room in user_and_rooms.rooms
                    rooms.push {room_id:room.id, name:room.name}
                chat_room_members(room_id).then (room) ->
                    for member in room.users
                        members.push {user_id: member.id, name:member.first_name }
                    members.push {user_id: room.owner.user_id, name:room.owner.first_name}
                    deferred.resolve({room_id:room_id, messages:reply, user_id:user.id, name:user.name, chat_rooms:rooms, members:members } )

strategy =(params) ->
    deferred = Q.defer()
    if params.timestamp?
        redis.zrevrangebyscore "room:#{params.room_id}:messages", '+inf', params.timestamp, create_callback(deferred, params.room_id, params.user)
    else
        redis.zrevrange "room:#{params.room_id}:messages", 0, 49, create_callback(deferred, params.room_id, params.user)
    deferred.promise
module.exports = strategy
