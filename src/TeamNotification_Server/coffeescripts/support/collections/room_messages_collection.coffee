class RoomMessagesCollection    
    constructor: (@room_messages) ->
        if @room_messages == null
            @room_messages = []
        else
            @room_messages.reverse()  #&& @room_messages.lenght > 0

    to_json: ->
        get_data_for = (message) ->
            return {
                "data": [
                    { 'name':'user', 'value': message.user.name}
                    { 'name':'body', 'value': JSON.parse(message.body).message} 
                    { 'name':'datetime', 'value':message.date }
                ]
            }
        m  = []
        for message in @room_messages
            m.push get_data_for message
        console.log m
        return m
    #'links' : [
    #        {"name": "self", "rel": "Room Messages", 'href':"/room/#{room_id}/messages"}
    #    ]
    #    'messages':[
    #        { href:'',
    #          data:[ 
    #            { name:'user' value:'etoribio'}
    #            { name:'body': value:'Hello'} 
    #            { name:'datetime' value:'2012-06-23 13:30' }
    #          ]
    #       }
    #    ]
    fetch_to: (callback) ->
        Q.when(@collection, callback)

module.exports = RoomMessagesCollection
