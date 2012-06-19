Q = require('q')
_ = require('underscore')
Repository = require('../repository')

class RoomCollection

    constructor: (room_id) ->
        @room_id = room_id
        console.log 'cons', room_id, @room_id
        @repository = new Repository('ChatRoom')
        @collection = @repository.find({id: room_id}).then(@set_collection)
        _.bindAll @

    set_collection: (chat_rooms) ->
        users = ({"name": user.name, "rel": "User", "href": "/user/#{user.id}"} for user in chat_rooms[0].users)

        return {
            members: [
                {"href": "/room/#{@room_id}/users", "data": users}
            ]
            links: [
                {"name":"self", "rel": "Room", "href": "/room/#{@room_id}"}
                {"name": "Manage Members", "rel": "RoomMembers", "href": "/room/#{@room_id}/users"}
            ]
        }

    fetch_to: (callback) ->
        Q.when(@collection, callback)

module.exports = RoomCollection
