db_config = require('../config').db

orm = require('./orm_gateway').open()

_entity = {}
orm.then (db) ->
    _entity.ChatRoom = db.define 'chat_room',
        name : {type: 'string'}
    _entity.User = db.define 'users',
        first_name : {type: 'string'},
        last_name : {type: 'string'},
        email : {type: 'string'},
        password : {type: 'string'}
    _entity.ChatRoomMessage = db.define 'chat_room_messages'
        body : {type: 'string', length:1000}
        date : {type:'date', default:'now()'}

    _entity.ChatRoomUser = db.define 'chat_room_users'
        chat_room_id : {type: 'int'}
        user_id : {type:'int'}

    _entity.ChatRoomInvitation = db.define 'chat_room_invitation'
        id : {type: 'int'}
        chat_room_id : {type: 'int'}
        email : {type:'string', length:140}
        accepted : {type:'bit', default:'0'}
        date : {type:'date', default:'now()'}

    _entity.ChatRoom.hasOne('owner', _entity.User, {autoFetch: true})
    _entity.ChatRoom.hasMany('users', _entity.User, 'user', {autoFetch: true})
    #_entity.ChatRoom.hasMany('messages', _entity.ChatRoomMessages, 'message', {autoFetch: true})

    _entity.ChatRoomMessage.hasOne('user', _entity.User, 'user', {autoFetch: true})
    _entity.ChatRoomMessage.hasOne('room',_entity.ChatRoom,'room', {autoFetch: true})
    
    _entity.ChatRoomInvitation.hasOne('user',_entity.User,'user',{autoFetch:true})
    _entity.ChatRoomInvitation.hasOne('room',_entity.ChatRoom,'chat_room',{autoFetch:true})
    #_entity.ChatRoomMessage.sync()
    #_entity.ChatRoomInvitation.sync()

module.exports =
    core:
        entity_factory:
            create : (entity_name,params) ->
                return new _entity[entity_name](params)
            get: (entity_name) ->
                return _entity[entity_name]
