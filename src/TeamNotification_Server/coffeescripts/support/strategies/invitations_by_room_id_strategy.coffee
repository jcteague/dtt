Repository = require('../repository')
Q = require('q')
strategy = (room_id) ->
    new Repository('ChatRoomInvitation').find({chat_room_id:room_id}).then (invitations) ->
        return {"room_id": room_id, "result": invitations}
module.exports = strategy
