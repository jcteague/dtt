class UserInvitationsCollection
    
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
        self = {"name":"self", "rel": "Invitations", "href": "/user/#{@invitations.user_id}/invitations"}
        user = {"name":"user", "rel": "User", "href": "/user/#{@invitations.user_id}"}
        return {
            href: "/user/#{@invitations.user_id}/invitations"
            links:[ self, user ]
            invitations: get_data_for(invitation) for invitation in @invitations.result
            }
    
module.exports = UserInvitationsCollection
