Repository = require('../repository')
Q = require('q')
strategy = (user_id) ->
    new Repository('ChatRoomInvitation').find({user_id:user_id, accepted:0},'date desc').then (invitations) ->
        find_item = (arr, obj) ->
            i=0
            while i<arr.length
                if (arr[i].chat_room_id == obj.chat_room_id && arr[i].email == obj.email) 
                    return true
                i++
        filtered_invitations = []
        for invitation in invitations
            if !find_item(filtered_invitations,{email:invitation.email ,chat_room_id:invitation.chat_room_id})
                filtered_invitations.push invitation
        return {"user_id": user_id, "result": filtered_invitations}
module.exports = strategy
