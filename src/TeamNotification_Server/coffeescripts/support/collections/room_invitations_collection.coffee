class RoomInvitationsCollection
    
    constructor: (@invitations) ->
    
    to_json: () ->
    
        get_data_for = (invitation) ->
            return {
                "data": [
                    { "name":"email", 'value': invitation.email}
                    { "name":"accepted", 'value': invitation.accepted}
                    { "name":"chat_room_name", 'value': invitation.chat_room.name}
                    { "name":"chat_room_id", 'value': invitation.chat_room_id}
                    { "name":"date", 'value': invitation.date}
                ]
            }
        self = {"name":"self", "rel": "Invitations", "href": "/room/#{@invitations.room_id}/invitations"}
        room = {"name":"Room", "rel": "Room", "href": "/room/#{@invitations.room_id}"}
        return {
            href: "/room/#{@invitations.room_id}/invitations"
            links:[ self, room ]
            invitations: get_data_for(invitation) for invitation in @invitations.result
            }
    
module.exports = RoomInvitationsCollection
