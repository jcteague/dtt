config = require('../../config')()

class UserCollection

    constructor: (@data) ->
    to_json: ->
        self = "/user/#{@data.user_id}"
        invitations = (@get_data_for(invitation) for invitation in @data.invitations)
        rooms = (@get_room(room) for room in @data.rooms)
        
        template = 
            href: '/room'
            data:[
                {'name':'name', 'label':'New chatroom Name', 'type':'string'}
                {'name':'owner_id', 'label':'Owner Id', 'type':'hidden'}
            ]
        return {
            href: self
            user_id: @data.user_id
            #redis: config.redis
            invitations: invitations
            rooms: rooms
            template: template
            links: [
                {"rel":"User", "name": "self", "href": self}
                {"rel":"UserEdit", "name": "edit", "href": "/user/#{@data.user_id}/edit"}
                {"rel":"UserRooms", "name": "rooms", "href": "/user/#{@data.user_id}/rooms"}
                {"rel":"Room", "name": "Create Room", "href": "/room"}
                {"rel":"Invitations", "name": "Pending Invitations", "href": "/user/invitations" }
            ]
        }
    get_data_for: (invitation) ->
        return {
            "data": [
                { "name":"email", 'value': invitation.email}
                { "name":"accepted", 'value': invitation.accepted}
                { "name":"chat_room_name", 'value': invitation.chat_room.name}
                { "name":"chat_room_id", 'value': invitation.chat_room_id}
                { "name":"date", 'value': invitation.date}
            ]
        }
    get_room: (room) ->
        return {
            data: [
                {"name": "id", "value": room.id}
                {"name": "name", "value": room.name}
                {"name": "owner_id", "value": room.owner_id}
                {"name": "room_key", "value": if (room.owner_id == @data.user_id) then room.room_key else ""  }
            ],
            links: [
                {"rel": "Room", "name": room.name, "href": "/room/#{room.id}"}
            ]
        }


module.exports = UserCollection
