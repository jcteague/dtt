
config = require('../../config')()

class RoomMessagesCollection
    constructor: (@data) ->
        @room_messages = []
        @room_messages = @data.messages.reverse() unless @data.is_empty
    to_json: ->
        room_id = @data.room_id
        get_data_for = (message) ->
            return {
                "data": [
                    { 'name':'user_id', 'value': message.user_id}
                    { 'name':'user', 'value': message.name}
                    { 'name':'body', 'value': message.body} 
                    { 'name':'datetime', 'value':message.date }
                    { 'name':'stamp', 'value':message.stamp }
                ]
            }
        get_data_for_user = (user) ->
            return {
                "href": "/user/#{user.id}"
                "data": [
                    {"name": "id", "value": user.user_id}
                    {"name": "name", "value": user.name}
                ]
            }
        m = ( get_data_for(JSON.parse(message)) for message in @room_messages)
        r = []
        r.push(room) for room in @data.chat_rooms

        parsed_room_id = parseInt(room_id, 10)
        return {
            href: "/room/#{room_id}/messages"
            redis: config.redis
            links:[
                {"name": "self", "rel": "RoomMessages", 'href':"/room/#{room_id}/messages"}
                {"name": "Room", "rel": "Room", 'href':"/room/#{room_id}"}
                {"name": "User", "rel": "User", 'href':"/user/#{@data.user_id}"}
            ]
            template:
                'data':[
                    {'name':'message', 'label':'Send Message', 'type':'string-big'}
                ]
            messages: m
            room: (room for room in @data.chat_rooms when room.room_id is parsed_room_id)[0]
            user_rooms: r
            members: (get_data_for_user user for user in @data.members)
            user: {user_id:@data.user_id, name: @data.name}
        }

module.exports = RoomMessagesCollection
