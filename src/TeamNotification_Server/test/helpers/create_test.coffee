db_helper = require('./database_helper')

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

entities =
    ChatRoom: [
        {id: 1, name: 'foo', owner_id: 1}
        {id: 2, name: 'bar', owner_id: 1}
    ]
    User: [
        {id: 1, name: 'blah', email: 'foo@bar.com'}
    ]

db_helper.set_up_db(entities)
#db_helper.clean_up_db()
