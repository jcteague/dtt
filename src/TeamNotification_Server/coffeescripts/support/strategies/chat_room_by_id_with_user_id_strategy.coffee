Repository = require('../repository')

strategy = (parameters) ->
    console.log 'in strategy'
    {room_id: room_id, user_id: user_id} = parameters
    new Repository('ChatRoom').get_by_id(room_id).then (room) ->
        return {
            user_id: user_id
            room: room
        }

module.exports = strategy
