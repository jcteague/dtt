class RoomMembersCollection

    constructor: (room) ->
        @room = room

    to_json: ->
        get_data_for = (user) ->
            return {
                "href": "/user/#{user.id}"
                "data": [
                    {"name": "id", "value": user.id}
                    {"name": "name", "value": user.name}
                ]
            }
        return {
            members: (get_data_for user for user in @room.users)
            links: [{"name":"self", "rel": "RoomMembers", "href": "/room/#{@room.id}/users"}]
        }

module.exports = RoomMembersCollection
