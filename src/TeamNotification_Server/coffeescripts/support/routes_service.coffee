Q = require('q')
Repository = require('./repository')
CollectionActionResolver = require('./collection_action_resolver')

build = (collection_type) ->
    new CollectionActionResolver(collection_type)

add_user_to_chat_room = (user_id, room_id) ->
    # TODO: How can we make this instances live outside and created within require?
    user_repository = new Repository('User')
    chat_room_repository = new Repository('ChatRoom')

    user_id = parseInt(user_id, 10)
    defer = Q.defer()
    user_repository.get_by_id(user_id).then (user) ->
        if user?
            chat_room_repository.get_by_id(room_id).then (chat_room) ->
                if (member for member in chat_room.users when member.id is user_id).length is 0
                    chat_room.addUsers(user, () ->
                        defer.resolve({success: true, messages: ["user added"]})
                    )
                else
                    defer.resolve({success: false, messages: ["user is already in the room"]})
        else
            defer.resolve({success: false, messages: ["user does not exist"]})

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
