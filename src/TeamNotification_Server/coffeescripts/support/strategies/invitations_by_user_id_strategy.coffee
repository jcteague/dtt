Repository = require('../repository')
strategy = (user_id) ->
     new Repository('ChatRoomInvitation').find({user_id:user_id})
        
module.exports = strategy
