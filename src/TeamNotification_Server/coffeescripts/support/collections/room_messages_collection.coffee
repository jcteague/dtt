
class RoomMessagesCollection    
    constructor: (@room_messages) ->
        @room_id = @get_room_id()
        @room_messages.sort( (a,b) ->  JSON.parse(a).date > JSON.parse(b).date) unless @room_messages.is_empty

    to_json: ->
        
        get_data_for = (message) ->
            return {
                "data": [
                    { 'name':'user', 'value': message.name}
                    { 'name':'body', 'value': JSON.parse(message.body).message} 
                    { 'name':'datetime', 'value':message.date }
                ]
            }
        m = ( get_data_for(JSON.parse(message)) for message in @room_messages)

        return{  
                href: "/room/#{@room_id}/messages" 
                links:[
                    {"name": "self", "rel": "RoomMessages", 'href':"/room/#{@room_id}/messages"}
                    {"name": "Room", "rel": "Room", 'href':"/room/#{@room_id}"}
                ]
                template: 'data':[
                    {'name':'message', 'label':'Message', 'type':'string-big', 'maxlength':100}
                ]
                messages: m
            }
    get_room_id: ->
        if @room_messages.is_empty
            return @room_messages.room_id
        else
            return JSON.parse(@room_messages[0]).room_id

module.exports = RoomMessagesCollection
