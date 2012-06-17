methods = {}
support = require('support').core
express = require('express')

methods.post_room = (req, res, next) ->
    values = req.body
    chat_room = support.entity_factory.create('ChatRoom',values)
    chat_room.save (err,saved_chat_room) ->
        if !err
            res.send('room '+ saved_chat_room.id + ' created')
        else 
            next(new Error(err.code,err.message))

methods.get_room_by_id = (req, res) ->
    r =
        links: [ 
            {"rel":"self", "href": "/rooms/1" } 
        ] 
    res.json(r)

methods.get_room = (req, res) ->
    r =
        'links' : [
          {'rel':'self', 'href':'/room'}
        ]
        'template':
            'data':[
                {'name':'name', 'label':'Chatroom Name', 'type':'string'}
            ]
    res.json(r)

module.exports =
    methods: methods,
    build_routes: (app) ->
        app.post('/room',express.bodyParser(), methods.post_room)
        app.get('/rooms/:id',methods.get_room_by_id)
        app.get('/room',methods.get_room)
