class RoomMessagesCollection

    constructor: (@room_messages) ->
        console.log @room_messages

    to_json: ->
        get_data_for = (message) ->
            return {
                "data": [
                    { 'name':'user', 'value': message.user.name}
                    { 'name':'body', 'value': message.body} 
                    { 'name':'datetime', 'value':message.date }
                ]
            }
        
        return { 
            links:[
                {"name": "self", "rel": "Room Messages", 'href':"/room/#{@room_messages[0].room_id}/messages"}
                {"name": "Room", "rel": "Room", 'href':"/room/#{@room_messages[0].room_id}"}
            ]
            messages:[ get_data_for message for message in @room_messages ]
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
