users = 
    name: 'users'
    columns:
        id: 'serial'
        first_name: 'varchar(100)'
        last_name: 'varchar(100)'
        email: 'varchar(100)'
        password: 'varchar(100)'

chat_rooms =
    name: 'chat_room'
    columns:
        id: 'serial'
        name: 'varchar(100)'
        owner_id: 'integer'

chat_room_users =
    name: 'chat_room_users'
    columns:
        chat_room_id: 'integer'
        user_id: 'integer'

chat_room_messages =
    name: 'chat_room_messages'
    columns:
        id: 'serial'
        body: 'varchar(100)'
        date: 'varchar(200)'
        user_id: 'integer'
        room_id: 'integer'

module.exports =
    users: users
    chat_rooms: chat_rooms
    chat_room_users: chat_room_users
    chat_room_messages: chat_room_messages
