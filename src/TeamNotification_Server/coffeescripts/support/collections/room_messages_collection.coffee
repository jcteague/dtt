class RoomMessagesCollection    
    constructor: (@room_messages) ->
        @room_messages.reverse() unless @room_messages.is_empty

    to_json: ->
        room_id = @get_room_id()
        get_data_for = (message) ->
            return {
                "data": [
                    { 'name':'user', 'value': message.user.name}
                    { 'name':'body', 'value': JSON.parse(message.body).message} 
                    { 'name':'datetime', 'value':message.date }
                ]
            }
        m = (get_data_for(message) for message in @room_messages)

        return {
            href: "/room/#{room_id}/messages"
            links:[
                {"name": "self", "rel": "RoomMessages", 'href':"/room/#{room_id}/messages"}
                {"name": "Room", "rel": "Room", 'href':"/room/#{room_id}"}
            ]
            template:
                'data':[
                    {'name':'message', 'label':'Message', 'type':'string-big', 'maxlength':100}
                ]
            messages: m
        }

    get_room_id: ->
        if @room_messages.is_empty
            @room_messages.args[0][0].room_id
        else
            @room_messages[0].room_id

module.exports = RoomMessagesCollection
