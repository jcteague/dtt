Q = require('q')
Repository = require('./repository')

user_repository = new Repository('User')
chat_room_invitation_repository = new Repository('ChatRoomInvitation')
chat_room_user_repository = new Repository('ChatRoomUser')

mark_as_accepted = (user, invitation) ->
    invitation.accepted = 1
    invitation.save (err, updated) -> null
    {user_id: user.id, chat_room_id: invitation.chat_room_id}

create_user = (user_data, invitation_updater = null) ->
    #invitation_updater = mark_invitations_as_accepted if invitation_updater
    created_user = null
    Q.fcall(() ->
        user_repository.save(user_data)
    ).then((user) ->
        created_user = user
        chat_room_invitation_repository.find(email: user.email).then (invitations) ->
            if invitations?
                return invitation_updater(invitations)
            []
    ).then((user_room_pair_array) ->
        (chat_room_user_repository.save user_and_room for user_and_room in user_room_pair_array)
        created_user
    )

module.exports =
    create_user: create_user
