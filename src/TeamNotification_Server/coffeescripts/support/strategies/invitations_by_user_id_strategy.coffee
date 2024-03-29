Repository = require('../repository')
Q = require('q')
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
        return {"user_id": user_id, "result": filtered_invitations}
module.exports = strategy
