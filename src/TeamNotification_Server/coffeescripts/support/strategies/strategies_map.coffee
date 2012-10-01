mapping =
    'root_collection': 'pass_through_strategy'
    'registration_collection': 'null_strategy'
    'user_collection': 'chat_rooms_by_owner_id_or_member_strategy'
    'user_edit_collection': 'user_by_id_strategy'
    'user_login_collection': 'user_by_login_strategy'
    'users_collection': 'user_by_email_strategy'
    'user_rooms_collection': 'chat_rooms_by_owner_id_or_member_strategy'
    'room_collection': 'chat_room_by_id_with_user_id_strategy'
    'room_members_collection': 'chat_room_by_id_strategy'
    'room_messages_collection': 'chat_room_messages_by_chat_room_id_strategy'
    'user_invitations_collection': 'invitations_by_user_id_strategy'
    'room_invitations_collection': 'invitations_by_room_id_strategy'
    'github_repositories_collection': 'github_repositories_by_auth_token_strategy'

module.exports = mapping
