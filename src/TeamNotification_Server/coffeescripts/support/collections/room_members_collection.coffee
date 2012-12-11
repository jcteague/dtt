class RoomMembersCollection

    constructor: (room) ->
        @room = room

    to_json: ->
        get_data_for = (user, room) ->
            return {
                "href": "/user/#{user.id}"
                "data": [
                    {"name": "id", "value": user.id}
                    {"name": "name", "value": user.first_name}
                    {"name": "room_id", "value": room.id}
                    {"name": "room_name", "value": room.name}
                ]
            }
        self = "/room/#{@room.id}/users"
        queries = [{
          "href" : "/users/query"
          "rel" : "users"
          "prompt" : "Enter search string"
          "type" : "autocomplete"
          "submit": self
          "data" :[{"name" : "email", "value" : ""}]
        }]
        return {
            href: self
            members: (get_data_for(user, @room) for user in @room.users)
            links: [{"name":"self", "rel": "RoomMembers", "href": "/room/#{@room.id}/users"},{"name":"Room", "rel": "Room", "href": "/room/#{@room.id}"}]
            queries: queries
        }

module.exports = RoomMembersCollection
