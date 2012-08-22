redis = require('redis').createClient()
Q = require('q')
chat_rooms = require('chat_rooms_by_owner_id_or_member_strategy')

message = null
callback = (deferred, room_id) ->
    return (err, reply) ->
        if !err
            if reply.length > 0
                deferred.resolve(reply)
            else
                deferred.resolve 
                    is_empty: true
                    room_id: room_id
strategy = (room_id, user_id) ->
    deferred = Q.defer()
    redis.zrevrange "room:#{room_id}:messages", 0, 49, callback(deferred, room_id)
    deferred.promise
module.exports = strategy

