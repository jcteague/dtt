class RoomMembersCollection

    constructor: (room) ->
        @room = room

    to_json: ->
        get_data_for = (user) ->
            return {
                "href": "/user/#{user.id}"
                "data": [
                    {"name": "id", "value": user.id}
                    {"name": "name", "value": user.first_name}
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

        if @room? and @room.users?
            members = (get_data_for user for user in @room.users)
        else
            members = []

        return {
            href: self
            members: members
            links: [{"name":"self", "rel": "RoomMembers", "href": "/room/#{@room.id}/users"},{"name":"Room", "rel": "Room", "href": "/room/#{@room.id}"}]
            queries: queries
        }

module.exports = RoomMembersCollection
