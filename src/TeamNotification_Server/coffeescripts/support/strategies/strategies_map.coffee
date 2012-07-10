mapping =
    'root_collection': 'pass_through_strategy'
    'user_collection': 'chat_rooms_by_owner_id_or_member_strategy'
    'users_collection': 'user_by_username_strategy'
    'user_rooms_collection': 'chat_rooms_by_owner_id_or_member_strategy'
    'room_collection': 'chat_room_by_id_with_user_id_strategy'
    'room_members_collection': 'chat_room_by_id_strategy'
    'room_messages_collection': 'chat_room_messages_by_chat_room_id_strategy'

module.exports = mapping
