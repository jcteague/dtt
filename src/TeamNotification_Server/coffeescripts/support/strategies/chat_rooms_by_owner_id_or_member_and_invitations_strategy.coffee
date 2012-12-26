Repository = require('../repository')
Q = require('q')
_ = require('underscore')
strategy = (user_id) ->
    new Repository('ChatRoomInvitation').find({user_id:user_id, accepted:0},'date desc').then (invitations) ->
        filtered_invitations = []
        if invitations?
            find_item = (arr, obj) ->
                for elem in arr
                    if (elem.chat_room_id == obj.chat_room_id && elem.email == obj.email) 
                        return true    
            for invitation in invitations
                if !find_item(filtered_invitations,{email:invitation.email ,chat_room_id:invitation.chat_room_id})
                    filtered_invitations.push invitation
        defer = Q.defer()
        new Repository('ChatRoom').find().then (rooms) ->
           # user_id = parseInt(user_id, 10)
            defer.resolve({
                user_id: user_id
                rooms: if rooms? then (room for room in rooms when room.owner_id is user_id or has_user(room, user_id)) else []
                invitations: filtered_invitations
            })
        defer.promise
has_user = (room, user_id) ->
    members = (user.id for user in room.users)
    members.indexOf(user_id) isnt -1

module.exports = strategy



