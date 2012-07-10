Repository = require('../repository')

strategy = (room_id, user_id) ->
    new Repository('ChatRoom').get_by_id(room_id).then (room) ->
        return {
            user_id: user_id
            room: room
        }

module.exports = strategy
