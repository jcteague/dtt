Repository = require('../repository')

strategy = (room_id) ->
    new Repository('ChatRoomMessage').find({room_id: room_id})

module.exports = strategy
