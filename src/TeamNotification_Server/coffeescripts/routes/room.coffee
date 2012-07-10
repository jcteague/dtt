methods = {}
support = require('../support/core').core
express = require('express')
routes_service = require('../support/routes_service')
build = routes_service.build

methods.user_authorized_in_room = (req, res, next) ->
    room_id = req.param('id')
    routes_service.is_user_in_room(req.user.id, room_id).then (is_user_in) ->
        if is_user_in 
            return next() 
        res.redirect '/'

methods.post_room = (req, res, next) ->
    values = req.body

    chat_room = support.entity_factory.create('ChatRoom', {name: values.name, owner_id: req.user.id})
    chat_room.save (err,saved_chat_room) ->
        if !err
            res.send('room '+ saved_chat_room.id + ' created')
        else 
            next(new Error(err.code,err.message))

methods.get_room_by_id = (req, res) ->
    room_id = req.param('id')
    callback = (collection) ->
        res.json(collection.to_json())

    build('room_collection').for(room_id: room_id, user_id: req.user.id).fetch_to callback

methods.post_room_user = (req, res, next) ->
    routes_service.add_user_to_chat_room(req.body.id, req.param('id')).then (response) ->
        res.send(response)

methods.manage_room_members = (req, res) ->
    room_id = req.param('id')
    callback = (collection) ->
        res.json(collection.to_json())

    build('room_members_collection').for(room_id).fetch_to callback

methods.get_room = (req, res) ->
    r =
        'href': '/room'
        'links' : [
          {"name": "self", "rel": "Room", 'href':'/room'}
        ]
        'template':
            'data':[
                {'name':'name', 'label':'Chatroom Name', 'type':'string'}
                {'name':'owner_id', 'label':'Owner Id', 'type':'hidden'}
            ]
    res.json(r)


methods.get_room_messages = (req,res) ->
    room_id = req.param('id')
    callback = (collection) ->
        res.json collection.to_json()

    build('room_messages_collection').for(room_id).fetch_to callback

methods.post_room_message = (req, res, next) ->
    values = req.body
    room_id = req.param('id')
    message_body = JSON.stringify({message:values.message})
    newMessage = {body: message_body, room_id:room_id, user_id: req.user.id, date:new Date()}
    room_message = support.entity_factory.create('ChatRoomMessage', newMessage)
    room_message.save (err,saved_message) ->
        if !err
            res.send({success:true, newMessage:saved_message})
        else 
            next(new Error(err.code,err.message))
    
module.exports =
    methods: methods,
    build_routes: (app) ->
        app.get('/room',methods.get_room)
        app.get('/room/:id', methods.user_authorized_in_room, methods.get_room_by_id)
        app.get('/room/:id/messages', methods.user_authorized_in_room, methods.get_room_messages)
        app.get('/room/:id/users', methods.user_authorized_in_room, methods.manage_room_members)
        app.post('/room/:id/messages', methods.user_authorized_in_room, express.bodyParser(), methods.post_room_message)
        app.post('/room',express.bodyParser(), methods.post_room)
        app.post('/room/:id/users', methods.user_authorized_in_room, express.bodyParser(), methods.post_room_user)
