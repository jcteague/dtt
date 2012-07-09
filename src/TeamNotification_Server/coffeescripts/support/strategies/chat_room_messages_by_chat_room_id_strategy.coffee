Repository = require('../repository')

strategy = (room_id) ->
    new Repository('ChatRoomMessage').find({room_id: room_id},'date desc',50).then (messages) ->
        return {
            room_id: room_id
            messages: if messages? then messages else []
        }

module.exports = strategy
