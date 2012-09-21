Q = require('q')
Repository = require('./repository')
mark_invitations_as_accepted = require('./invitations/invitations_status_updater').update

user_repository = new Repository('User')
chat_room_invitation_repository = new Repository('ChatRoomInvitation')
chat_room_user_repository = new Repository('ChatRoomUser')

create_user = (user_data) ->
    created_user = null
    Q.fcall(() ->
        user_repository.save(user_data)
    ).then((user) ->
        created_user = user
        chat_room_invitation_repository.find(email: user.email).then (invitations) ->
            if invitations?
                return mark_invitations_as_accepted(user, invitations)
            []
    ).then((user_room_pair_array) ->
        (chat_room_user_repository.save user_and_room for user_and_room in user_room_pair_array)
        created_user
    )

module.exports =
    create_user: create_user
