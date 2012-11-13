Q = require('q')
Repository = require('./repository')
mark_invitations_as_accepted = require('./invitations/invitations_status_updater').update
sha256 = require('node_hash').sha256

user_repository = new Repository('User')
chat_room_invitation_repository = new Repository('ChatRoomInvitation')
chat_room_user_repository = new Repository('ChatRoomUser')
user_confirmation_key_repository = new Repository('UserConfirmationKey')

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
    ).then (user) ->
        generate_key = (some_salty_stuff) ->
            d = new Date().getTime().toString()
            sha256(d+some_salty_stuff)
        new_confirmation_key =
            user_id: user.id
            confirmation_key: generate_key()
        user_confirmation_key_repository.save(new_confirmation_key)
        user.confirmation_key = new_confirmation_key.confirmation_key 
        return user

module.exports =
    create_user: create_user
