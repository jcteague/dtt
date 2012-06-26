_ = require('underscore')
integration = require('./integration')

schema = 
    ChatRoom:
        table: 'chat_room'
        columns:
            id: {type: 'integer'}
            name: {type: 'string'}
            owner_id: {type: 'integer'}
    User:
        table: 'users'
        columns:
            id: {type: 'integer'}
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
