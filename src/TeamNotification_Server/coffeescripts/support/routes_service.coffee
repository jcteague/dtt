Q = require('q')
Repository = require('./repository')
CollectionActionResolver = require('./collection_action_resolver')
email_sender = require('./email/email_sender')
email_template = require('./email/templates/email_template')
sha256 = require('node_hash').sha256

build = (collection_type) ->
    new CollectionActionResolver(collection_type)

generate_confirmation_key = (key) ->
    d = new Date().getTime().toString()
    console.log d
    sha256(d+key)
    
get_messages_from_flash = (flash_data) ->
    messages = []
    if(flash_data?)
        if flash_data.info?
            for info in flash_data.info
                messages.push(info)
        if flash_data.error?
            for error in flash_data.error
                messages.push(error)
    messages

add_user_to_chat_room = (current_user, email, room_id) ->
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
                        response = get_server_response(true, ["User added"], "/room/#{room_id}/users/")
                        defer.resolve(response)
                    )
                else
                    response = get_server_response(false, ["User #{user.email} is already in the room"], "/user/#{user.id}/")
                    defer.resolve(response)
        else
            chat_room_repository.get_by_id(room_id).then (chat_room) ->
                chat_room_invitation_repository.save({email:email, chat_room_id:room_id, user_id: current_user.id})
                
                template = email_template.for.invitation.using
                    email: email
                    chat_room: chat_room
                email_sender.send template
                response = get_server_response(false, ["An email invitation has been sent to #{email}"], "/room/#{room_id}/invitations/")
                defer.resolve(response)

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

get_server_response = (success, messages, link='', redirect=false) ->
    return {success:success, server_messages:messages, link:link, redirect:redirect}


module.exports =
    build: build
    get_server_response: get_server_response
    add_user_to_chat_room: add_user_to_chat_room
    is_user_in_room: is_user_in_room
    get_messages_from_flash: get_messages_from_flash
    generate_confirmation_key: generate_confirmation_key
