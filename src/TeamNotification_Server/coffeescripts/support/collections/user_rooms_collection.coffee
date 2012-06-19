Q = require('q')
_ = require('underscore')
Repository = require('../repository')

class UserRoomsCollection

    constructor: (user_id) ->
        @repository = new Repository 'ChatRoom'
        @collection = @repository.find({owner_id: user_id}).then(@set_collection)
        @user_id = user_id
        _.bindAll @
    
    set_collection: (chat_rooms) ->
        
        parsed_links = []
        parsed_links.push( {'name':'self', 'rel':'self', 'href':"/user/#{@user_id}/rooms"} )
        
        for room in chat_rooms
            parsed_links.push( {"name":"#{room.name}", "rel": room.name, "href": "/room/#{room.id}" })
        
        return {links:parsed_links}

    fetch_to: (callback) ->
        Q.when(@collection, callback)

module.exports = UserRoomsCollection
