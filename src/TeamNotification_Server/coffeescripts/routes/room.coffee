methods = {}
support = require('../support/core').core
express = require('express')
routes_service = require('../support/routes_service')
build = routes_service.build

methods.post_room = (req, res, next) ->
    values = req.body

    # TODO: Get the owner id value from the actual logged in user
    chat_room = support.entity_factory.create('ChatRoom', {name: values.name, owner_id: 1})
    chat_room.save (err,saved_chat_room) ->
        if !err
            res.send('room '+ saved_chat_room.id + ' created')
        else 
            next(new Error(err.code,err.message))

methods.get_room_by_id = (req, res) ->
    room_id = req.param('id')
    callback = (collection) ->
        console.log collection, collection.to_json()
        #res.json(collection)
        res.json(collection.to_json())

    build('room_collection').for(room_id).fetch_to callback

methods.post_room_user = (req, res, next) ->
    routes_service.add_user_to_chat_room(req.body.user_id, req.param('id')).then (response) ->
        res.send(response)

methods.manage_room_members = (req, res) ->
    room_id = req.param('id')
    callback = (collection) ->
        res.json(collection)

    build('room_members_collection').for(room_id).fetch_to callback

methods.get_room = (req, res) ->
    r =
        'links' : [
          {"name": "self", "rel": "Room", 'href':'/room'}
        ]
        'template':
            'data':[
                {'name':'name', 'label':'Chatroom Name', 'type':'string'}
                {'name':'owner_id', 'label':'Owner Id', 'type':'hidden'}
            ]
    res.json(r)

module.exports =
    methods: methods,
    build_routes: (app) ->
        app.get('/room',methods.get_room)
        app.get('/room/:id',methods.get_room_by_id)
        app.get('/room/:id/users',methods.manage_room_members)
        app.post('/room',express.bodyParser(), methods.post_room)
        app.post('/room/:id/users',express.bodyParser(), methods.post_room_user)
