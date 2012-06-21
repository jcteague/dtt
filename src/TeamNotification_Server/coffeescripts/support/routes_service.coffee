Q = require('q')
Repository = require('./repository')
CollectionActionResolver = require('./collection_action_resolver')

build = (collection_type) ->
    new CollectionActionResolver(collection_type)

add_user_to_chat_room = (user_id, room_id) ->
    # TODO: How can we make this instances live outside and created within require?
    user_repository = new Repository('User')
    chat_room_repository = new Repository('ChatRoom')

    defer = Q.defer()
    user_repository.get_by_id(user_id).then (user) ->
        if user?
            chat_room_repository.get_by_id(room_id).then (chat_room) ->
                chat_room.addUsers(user, () ->
                    defer.resolve("user #{user.id} added")
                )
        else
            defer.resolve("user #{user_id} does not exist")
    defer.promise

module.exports =
    build: build
    add_user_to_chat_room: add_user_to_chat_room
