Repository = require('../repository')
strategy = (user_id) ->
     new Repository('ChatRoomInvitation').find(user_id).then (invitations) ->
        invitations
        
module.exports = strategy
