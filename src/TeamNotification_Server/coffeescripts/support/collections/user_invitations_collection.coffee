class UserInvitationsCollection
    constructor: (@user_invitations) ->
    
    to_json: () ->
 #       if @userinvitations
  #      self = {"name":"self", "rel": "Invitations", "href": "/user/#{@room.room.id}"}
        
        return {}
    
module.exports = UserInvitationsCollection
