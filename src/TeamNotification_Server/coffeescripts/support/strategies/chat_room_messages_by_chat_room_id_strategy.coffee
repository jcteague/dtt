#Repository = require('../repository')
redis = require('redis').createClient()
Q = require('q')

message = null
callback = (deferred, room_id) ->
    return (err, reply) ->
        if !err
            if reply?
                deferred.resolve(reply)
            else
                deferred.resolve 
                    is_empty: true
                    room_id: room_id
strategy = (room_id) ->
    #new Repository('ChatRoomMessage').find({room_id: room_id},'date desc',50)
    deferred = Q.defer()
    redis.smembers "room:#{room_id}:messages", callback(deferred, room_id)
    #redis.quit()
    deferred.promise
module.exports = strategy
