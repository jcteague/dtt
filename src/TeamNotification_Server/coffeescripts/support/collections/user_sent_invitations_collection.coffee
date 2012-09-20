class UserSentInvitationsCollection
    constructor: (@userinvitations) ->
    
    to_json: () ->
 #       if @userinvitations
  #      self = {"name":"self", "rel": "Invitations", "href": "/user/#{@room.room.id}"}
        
        return {}
    
module.exports = UserSentInvitationsCollection
