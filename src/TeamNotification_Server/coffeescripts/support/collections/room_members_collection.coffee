Q = require('q')
_ = require('underscore')
Repository = require('../repository')

class RoomMembersCollection

    constructor: (room_id) ->
        @room_id = room_id
        @repository = new Repository('ChatRoom')
        _.bindAll @
        @collection = @repository.find(id: room_id).then(@set_collection)

    set_collection: (chat_rooms) ->
        get_data_for = (user) ->
            return {
                "href": "/user/#{user.id}"
                "data": [
                    {"name": "id", "value": user.id}
                    {"name": "name", "value": user.name}
                ]
            }
        room_id = @room_id
        return {
            members: (get_data_for user for user in chat_rooms[0].users)
            links: [{"name":"self", "rel": "RoomMembers", "href": "/room/#{@room_id}/users"}]
        }

    fetch_to: (callback) ->
        Q.when(@collection, callback)

module.exports = RoomMembersCollection
