class UserRoomsCollection

    constructor: (@rooms) ->

    to_json: ->
        owner_id = if @rooms.is_empty then @rooms.args[0][0].owner_id else @rooms[0].owner_id
        self_link = {'name':'self', 'rel':'self', 'href':"/user/#{owner_id}/rooms"}
        room_links = ({"name":"#{room.name}", "rel": room.name, "href": "/room/#{room.id}"} for room in @rooms when room.name?)
        
        return {
            links: [self_link].concat room_links
        }

    fetch_to: (callback) ->
        Q.when(@collection, callback)

module.exports = UserRoomsCollection
