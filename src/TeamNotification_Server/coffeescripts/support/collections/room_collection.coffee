class RoomCollection

    constructor: (@room) ->

    to_json: ->
        users = ({"name": user.name, "rel": "User", "href": "/user/#{user.id}"} for user in @room.room.users)
        self = {"name":"self", "rel": "Room", "href": "/room/#{@room.room.id}"}
        other_links = []
        if @room.user_id is @room.room.owner_id
            other_links.push {"name": "Manage Members", "rel": "RoomMembers", "href": "/room/#{@room.room.id}/users"}
        other_links.push {"name": "Room Messages", "rel": "RoomMessages", "href": "/room/#{@room.room.id}/messages"}
  
        return {
            href: self
            members: [
                {"href": "/room/#{@room.room.id}/users", "data": users}
            ]
            links: [self].concat(other_links)
        }

module.exports = RoomCollection
