config = require('../../config')()

class UserCollection

    constructor: (@data) ->

    to_json: ->
        self = "/user/#{@data.user_id}"
        invitations = "/user/#{@data.user_id}/invitations"
        rooms = (@get_room(room) for room in @data.rooms)
        return {
            href: self
            redis: config.redis
            rooms: rooms
            links: [
                {"rel":"User", "name": "self", "href": self}
                {"rel":"UserEdit", "name": "edit", "href": "/user/#{@data.user_id}/edit"}
                {"rel":"UserRooms", "name": "rooms", "href": "/user/#{@data.user_id}/rooms"}
                {"rel":"Room", "name": "Create Room", "href": "/room"}
                {"rel":"Invitations", "name": "Sent Invitations", "href": invitations }
            ]
        }

    get_room: (room) ->
        return {
            data: [
                {"name": "id", "value": room.id}
                {"name": "name", "value": room.name}
            ],
            links: [
                {"rel": "Room", "name": room.name, "href": "/room/#{room.id}"}
            ]
        }


module.exports = UserCollection
