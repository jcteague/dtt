Repository = require('../repository')

strategy = (parameters) ->
    new Repository('ChatRoom').find({owner_id:parameters.owner_id}).then (rooms) ->
        clear_room = (room) ->
            return {room_id:room.id, name:room.name, room_key:room.room_key }
        return {
            owner_id: parameters.owner_id
            rooms: (clear_room(room) for room in rooms )
        }

module.exports = strategy
