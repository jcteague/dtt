Repository = require('../repository')

strategy = (room_id) ->
    new Repository('ChatRoomMessage').find({room_id: room_id},'date desc',50)

module.exports = strategy
