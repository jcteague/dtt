Q = require('q')
Repository = require('../repository')

class RoomMembersCollection

    constructor: (room_id) ->
        @room_id = room_id
        @repository = new Repository('ChatRoom')
        @collection = @repository.find(id: room_id).then(@set_collection)

    set_collection: (chat_rooms) =>
        self = {"name":"self", "rel": "RoomMembers", "href": "/room/#{@room_id}"}
        users = ({"name": user.name, "rel": "User", "href": "/user/#{user.id}"} for user in chat_rooms[0].users)
        return {
            links: [self].concat(users)
        }

    fetch_to: (callback) ->
        Q.when(@collection, callback)

module.exports = RoomMembersCollection
