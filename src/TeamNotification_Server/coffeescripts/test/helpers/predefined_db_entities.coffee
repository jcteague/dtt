users = 
    name: 'users'
    columns:
        id: 'integer'
        name: 'varchar(100)'
        email: 'varchar(100)'

chat_rooms =
    name: 'chat_room'
    columns:
        id: 'integer'
        name: 'varchar(100)'
        owner_id: 'integer'

module.exports =
    users: users
    chat_rooms: chat_rooms
