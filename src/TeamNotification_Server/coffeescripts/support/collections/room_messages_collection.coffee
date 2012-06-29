class RoomMessagesCollection    
    constructor: (@room_messages) ->
        console.log @room_messages

    to_json: ->
        get_data_for = (message) ->
            console.log JSON.parse(message.body)
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
        return { 
            links:[
                {"name": "self", "rel": "Room Messages", 'href':"/room/#{@room_messages[0].room_id}/messages"}
                {"name": "Room", "rel": "Room", 'href':"/room/#{@room_messages[0].room_id}"}
            ]
            messages: m
        }
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
