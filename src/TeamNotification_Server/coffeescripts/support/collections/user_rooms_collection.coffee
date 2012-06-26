class UserRoomsCollection

    constructor: (@rooms) ->

    to_json: ->
        parsed_links = []
        parsed_links.push( {'name':'self', 'rel':'self', 'href':"/user/#{@rooms[0].owner_id}/rooms"} )
        
        for room in @rooms
            parsed_links.push( {"name":"#{room.name}", "rel": room.name, "href": "/room/#{room.id}" })
        
        return {links:parsed_links}

    fetch_to: (callback) ->
        Q.when(@collection, callback)

module.exports = UserRoomsCollection
