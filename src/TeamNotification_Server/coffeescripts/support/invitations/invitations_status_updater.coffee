mark_as_accepted = (user, invitation) ->
    invitation.accepted = 1
    invitation.save (err, updated) -> null
    {user_id: user.id, chat_room_id: invitation.chat_room_id}

update = (user, invitations) ->
    (mark_as_accepted(user, invitation) for invitation in invitations)

module.exports = 
    update: update
