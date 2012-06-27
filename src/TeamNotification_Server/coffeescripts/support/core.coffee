pg = require('pg')
db_config = require('../config').db

orm = require('./orm_gateway').open()

###
_entity = {}
db = orm.connect db_config.connection_string, (success,db) ->
    _entity.ChatRoom = db.define 'chat_room',
        name : {type: 'string'}
    _entity.User = db.define 'users',
        name : {type: 'string'},
        email : {type: 'string'}

    _entity.ChatRoom.hasOne('owner', _entity.User, {autoFetch: true})
    _entity.ChatRoom.hasMany('users', _entity.User, 'user', {autoFetch: true})
###
_entity = {}
orm.then (db) ->
    _entity.ChatRoom = db.define 'chat_room',
        name : {type: 'string'}
    _entity.User = db.define 'users',
        name : {type: 'string'},
        email : {type: 'string'}

    _entity.ChatRoom.hasOne('owner', _entity.User, {autoFetch: true})
    _entity.ChatRoom.hasMany('users', _entity.User, 'user', {autoFetch: true})

module.exports =
    core:
        entity_factory:
            create : (entity_name,params) ->
                return new _entity[entity_name](params)
            get: (entity_name) ->
                return _entity[entity_name]
