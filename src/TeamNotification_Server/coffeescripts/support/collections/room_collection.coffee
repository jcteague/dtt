class RoomCollection

    constructor: (room) ->
        @room = room

    to_json: ->
        users = ({"name": user.name, "rel": "User", "href": "/user/#{user.id}"} for user in @room.users)

        return {
            members: [
                {"href": "/room/#{@room.id}/users", "data": users}
            ]
            links: [
                {"name":"self", "rel": "Room", "href": "/room/#{@room.id}"}
                {"name": "Manage Members", "rel": "RoomMembers", "href": "/room/#{@room.id}/users"}
            ]
        }

module.exports = RoomCollection
