Q = require('q')
Repository = require('./repository')
CollectionActionResolver = require('./collection_action_resolver')
email_sender = require('./email/email_sender')
email_template = require('./email/templates/email_template')

build = (collection_type) ->
    new CollectionActionResolver(collection_type)

add_user_to_chat_room = (email, room_id) ->
    # TODO: How can we make this instances live outside and created within require?
    user_repository = new Repository('User')
    chat_room_repository = new Repository('ChatRoom')
    chat_room_invitation_repository = new Repository('ChatRoomInvitation')
    defer = Q.defer()
    user_repository.find(email:email).then (users) ->
        if users?
            user = users[0]
            chat_room_repository.get_by_id(room_id).then (chat_room) ->
                if (member for member in chat_room.users when member.id is user.id).length is 0 and chat_room.owner_id isnt user.id
                    chat_room.addUsers(user, () ->
                        defer.resolve({success: true, messages: ["user added"]})
                    )
                else
                    defer.resolve({success: false, messages: ["user is already in the room"]})
        else
            chat_room_repository.get_by_id(room_id).then (chat_room) ->
                template = email_template.for.invitation.using email, chat_room
                email_sender.send template
                defer.resolve({success: false, messages: ["An email invitation has been sent to #{email}"]})

    defer.promise

is_user_in_room = (user_id, room_id) ->
    user_repository = new Repository('User')
    chat_room_repository = new Repository('ChatRoom')

    user_id = parseInt(user_id, 10)
    defer = Q.defer()
    user_repository.get_by_id(user_id).then (user) ->
        chat_room_repository.get_by_id(room_id).then (chat_room) ->
            defer.resolve(chat_room.owner_id is user_id or (member for member in chat_room.users when member.id is user_id).length isnt 0)

    defer.promise

module.exports =
    build: build
    add_user_to_chat_room: add_user_to_chat_room
    is_user_in_room: is_user_in_room
