class UserRoomsCollection

    constructor: (@rooms) ->

    to_json: ->
        self_link = {'name':'self', 'rel':'self', 'href':"/user/#{@rooms.user_id}/rooms"}
        room_links = ({"name":"#{room.name}", "rel": room.name, "href": "/room/#{room.id}"} for room in @rooms.rooms)
        
        return {
            links: [self_link].concat room_links
        }

    fetch_to: (callback) ->
        Q.when(@collection, callback)

module.exports = UserRoomsCollection
