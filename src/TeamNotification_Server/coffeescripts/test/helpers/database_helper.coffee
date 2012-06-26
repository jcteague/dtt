_ = require('underscore')
integration = require('./integration')

schema = 
    ChatRoom:
        table: 'chat_room'
        columns:
            name: {type: 'string'}
    User:
        table: 'users'
        columns:
            name: {type: 'string'}
            email: {type: 'string'}

    relations: [
        {entity_name: 'ChatRoom', type: 'hasOne', relation_name: 'owner', related_entity: 'User'}
        {entity_name: 'ChatRoom', type: 'hasMany', relation_name: 'users', related_entity: 'User', relation_column: 'user'}
    ]

set_up_db = _.bind integration.set_up_db, {}, schema

module.exports =
    set_up_db: set_up_db
    clean_up_db: integration.clean_up_db
