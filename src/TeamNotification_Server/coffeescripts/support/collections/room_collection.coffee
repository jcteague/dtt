RoomMembersQueryBuilder = require('./room_members_query_builder')

class RoomCollection

    constructor: (@room) ->
    to_json: ->
        get_member_data = (user, room) ->
            return {
                "href": "/user/#{user.id}"
                "data": [
                    {"name": "id", "value": user.id}
                    {"name": "name", "value": user.first_name}
                    {"name": "room_id", "value": room.id}
                    {"name": "room_name", "value": room.name}
                ]
            }
        #users = ({"name": user.first_name, "rel": "User", "href": "/user/#{user.id}"} for user in @room.room.users)
        room_href = "/room/#{@room.room.id}/users"
        self = {"name":"self", "rel": "Room", "href": "/room/#{@room.room.id}"}
        members = (get_member_data(user, @room.room) for user in @room.room.users)
        other_links = []
        if @room.user_id is @room.room.owner_id
            other_links.push {"name":"Pending Invitations", "rel": "Invitations", "href": "/room/#{@room.room.id}/invitations"}
            other_links.push {"name": "Manage Members", "rel": "RoomMembers", "href": "/room/#{@room.room.id}/users"}
            other_links.push {"name":"Associate Repository", "rel": "Repository", "href": "/github/oauth", "external": true}
        other_links.push {"name": "Room Messages", "rel": "RoomMessages", "href": "/room/#{@room.room.id}/messages"}
        
        return {
            href:  room_href #self
            links: [self].concat(other_links)
            rooms: [@room_data(@room.room)]
            members: members
            queries: new RoomMembersQueryBuilder(room_href).get()
        }
    room_data: (room) ->
        links:[{ name:room.name, rel:"self", href: "/room/#{room.id}/"}]
        data:[{ name:'id', value:room.id},{ name:'name', value:room.name},{ name:'owner_id', value:room.owner_id}, { name:'room_key', value:if @room.user_id == room.owner_id then room.room_key else ""}]

module.exports = RoomCollection
