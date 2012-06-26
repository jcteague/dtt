db_helper = require('./database_helper')

entities =
    ChatRoom: [
        #{id: 1, name: 'foo', owner_id: 1}
        #{id: 2, name: 'bar', owner_id: 1}
        {name: 'foo', owner_id: 1}
        {name: 'bar', owner_id: 1}
    ]
    User: [
        {name: 'blah', email: 'foo@bar.com'}
    ]

db_helper.set_up_db(entities)
db_helper.clean_up_db('chat_room', 'users')
