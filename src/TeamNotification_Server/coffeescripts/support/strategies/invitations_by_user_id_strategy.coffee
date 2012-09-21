Repository = require('../repository')
Q = require('q')
strategy = (user_id) ->
    new Repository('ChatRoomInvitation').find({user_id:user_id}).then (invitations) ->
        return {"user_id": user_id, "result": invitations}
module.exports = strategy
