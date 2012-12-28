RoomMembersQueryBuilder = require('./room_members_query_builder')

class RoomMembersCollection

    constructor: (room)->
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
        queries = new RoomMembersQueryBuilder(self).get()
        members = (get_data_for(user, @room) for user in @room.users)
        return {
            href: "/room/#{@room.id}/users" #self
            members: members
            links: [{"name":"self", "rel": "RoomMembers", "href": "/room/#{@room.id}/users"},{"name":"Room", "rel": "Room", "href": "/room/#{@room.id}"}]
            queries: queries
        }
        
        
module.exports = RoomMembersCollection
