default_mapper = require('./default_collection_field_mapper.')

get_room_from = (room) ->
    return {
        "href": room.href
        "data": default_mapper.map(room)
    }

map = (rooms) ->
    (get_room_from user for room in rooms)

module.exports =
    map: map
