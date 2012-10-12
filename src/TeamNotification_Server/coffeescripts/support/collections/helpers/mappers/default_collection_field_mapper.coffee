#   users: [
#       {
#           id: "id"
#           name: "name"
#           href: "href",
#       },
#       {
#           id: "id"
#           name: "name"
#           href: "href",
#       }
#   ],
#   rooms: [
#       {
#           id: "id"
#           name: "name"
#           owner_id: "owner_id"
#           room_key: "room_key"
#           href: "href"
#       },
#       {
#           id: "id"
#           name: "name"
#           owner_id: "owner_id"
#           room_key: "room_key"
#           href: "href"
#       }
#   ],
#   messages: [
#       {
#           user_id: "user_id"
#           name: "name"
#           body: "body"
#           datetime: "datetime"
#           stamp: "stamp"
#       },
#       {
#           user_id: "user_id"
#           name: "name"
#           body: "body"
#           datetime: "datetime"
#           stamp: "stamp"
#       }
#   ],
#   invitations: [
#       {
#           email: "email"
#           chat_room_id: "chat_room_id"
#           chat_room_name: "chat_room_name"
#           date: "date"
#           accepted: "accepted"
#       },
#       {
#           email: "email"
#           chat_room_id: "chat_room_id"
#           chat_room_name: "chat_room_name"
#           date: "date"
#           accepted: "accepted"
#       }
#   ]
#

get_from = (key, value) ->
    if typeof value is 'object'
        return key_object_mapper(key, value)
    key_value_mapper(key, value)

key_value_mapper = (key, value) ->
    return {
        name: key
        value: value
    }

key_object_mapper = (key, value) ->
    obj = name: key
    for k, property of value
        obj[k] = property

    obj

map = (property_value) ->
    collection_data_value = (get_from(key, value) for key, value of property_value)
    return {
        data: collection_data_value
    }

module.exports =
    map: map
