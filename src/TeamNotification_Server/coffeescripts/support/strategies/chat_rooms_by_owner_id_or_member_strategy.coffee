Repository = require('../repository')
_ = require('underscore')

strategy = (user_id) ->
    new Repository('ChatRoom').find().then (rooms) ->
        user_id = parseInt(user_id, 10)
        return {
            user_id: user_id
            rooms: if rooms? then (room for room in rooms when room.owner_id is user_id or has_user(room, user_id)) else []
        }

has_user = (room, user_id) ->
    members = (user.id for user in room.users)
    members.indexOf(user_id) isnt -1

module.exports = strategy
