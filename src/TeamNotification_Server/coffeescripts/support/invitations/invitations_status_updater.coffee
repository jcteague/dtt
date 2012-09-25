_ = require('underscore')

mark_as_accepted = (invitation) ->
    invitation.accepted = 1
    invitation.save (err, updated) -> null
    invitation

update = (user, invitations) ->
    room_ids = _.uniq((mark_as_accepted(invitation).chat_room_id for invitation in invitations))
    ({chat_room_id: id, user_id: user.id} for id in room_ids)

module.exports = 
    update: update
