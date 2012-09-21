Repository = require('../repository')
Q = require('q')
strategy = (room_id) ->
    new Repository('ChatRoomInvitation').find({chat_room_id:room_id, accepted:0},'date desc').then (invitations) ->
        find_item = (arr, obj) ->
            i=0
            while i<arr.length
                if (arr[i].email == obj.email) 
                    return true
                i++
        filtered_invitations = []
        for invitation in invitations
            if !find_item(filtered_invitations,{email:invitation.email})
                filtered_invitations.push invitation 
        return {"room_id": room_id, "result": filtered_invitations}
module.exports = strategy
