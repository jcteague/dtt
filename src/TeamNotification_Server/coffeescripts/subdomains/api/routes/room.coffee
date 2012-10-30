methods = {}
support = require('../../../support/core').core
express = require('express')
sha256 = require('node_hash').sha256
routes_service = require('../../../support/routes_service')
config = require('../../../config')()
build = routes_service.build
redis_connector = require('../../../support/redis/redis_gateway')
socket_middleware = require('../../../support/middlewares').socket_io
get_server_response = routes_service.get_server_response
redis_subscriber = redis_connector.open()
redis_publisher = redis_connector.open()
redis_queryer = redis_connector.open()
logger = require('../../../support/logging/logger')
Repository = require('../../../support/repository')

methods.user_authorized_in_room = (req, res, next) ->
    room_id = req.param('id')
    routes_service.is_user_in_room(req.user.id, room_id).then (is_user_in) ->
        if is_user_in
            return next()

        res.json {"server_messages":["You're not subscribed to this room"]}

list_of_listeners = {}


methods.unsubscribe = (req,res) ->
    room_id = req.param('id')
    chat_room_user = new Repository('ChatRoomUser')
    chat_room_user.find(user_id:req.user.id, chat_room_id:room_id).then (chat_room_users) ->
        if (!chat_room_users)
            res.json get_server_response(false, ["The user is not subscribed to the room ):"], "" )
        else
            callback = () ->
                res.json get_server_response(true, ["Unsubscribed successfully"], "" )
            room_user = chat_room_users[0]
            room_user.remove callback

methods.get_room_invitations = (req, res) ->

    callback = (collection) ->
        res.json(collection.to_json())

    build('room_invitations_collection').for(req.param("id") ).fetch_to callback

methods.set_up_message_transmission = (io, room_id, listener_name) ->
    namespace_io = io.of(listener_name)
    room_channel = "chat #{room_id}"
    redis_subscriber.subscribe(room_channel)
    redis_subscriber.on "message", (channel, message) ->
        if(channel == "chat #{room_id}")
            namespace_io.send(message)

methods.set_socket_events = (io, room_id) ->
    listener_name = "/room/#{room_id}/messages"
    unless methods.is_listener_registered(listener_name)
        list_of_listeners[listener_name] = true
        methods.set_up_message_transmission(io, room_id, listener_name)

methods.is_listener_registered = (listener_name) ->
    list_of_listeners[listener_name]?

methods.post_room = (req, res, next) ->
    values = req.body
    room_key = sha256("room:#{values.name}")
    chat_room = support.entity_factory.create('ChatRoom', {name: values.name, owner_id: req.user.id, room_key:room_key })
    chat_room.save (err,saved_chat_room) ->
        if !err
            res.json( get_server_response(true, ["room #{saved_chat_room.id} created"], "/room/#{saved_chat_room.id}/",true ))
        else
            next(new Error(err.code,err.message))

methods.get_room_by_id = (req, res) ->
    room_id = req.param('id')
    callback = (collection) ->
        json = collection.to_json()
        res.json(json)

    build('room_collection').for(room_id: room_id, user_id: req.user.id).fetch_to callback

methods.post_room_user = (req, res, next) ->
    routes_service.add_user_to_chat_room(req.user, req.body.email, req.param('id')).then (response) ->
        res.send(response)

methods.manage_room_members = (req, res) ->
    room_id = req.param('id')
    callback = (collection) ->
        res.json(collection.to_json())

    build('room_members_collection').for(room_id).fetch_to callback

methods.get_accept_invitation = (req, res) ->
    email = req.param('email')
    callback = (collection) ->
        res.json(collection.fill(email: email).to_json())

    build('registration_collection').fetch_to callback

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
    user_id = req.user.id

    methods.set_socket_events(req.socket_io, room_id)
    callback = (collection) ->
        res.json collection.to_json()

    build('room_messages_collection').for({room_id:room_id, user:req.user}).fetch_to callback

methods.get_room_messages_since_timestamp = (req, res) ->
    room_id = req.param('id')
    console.log "Fetching messages since"
    console.log req.param('timestamp')
    console.log room_id
    callback = (collection) ->
        console.log "About to send the response"
        console.log collection.to_json()
        res.json collection.to_json()

    build('room_messages_collection').for({room_id:room_id, user:req.user, timestamp: req.param('timestamp')}).fetch_to callback


methods.post_room_message = (req, res, next) ->
    values = req.body
    if values.message != ''
        room_id = req.param('id')
        newMessage = ''
        message_date = ''
        message_stamp = ''
        setname = "room:#{room_id}:messages"
        if values.stamp? && values.stamp != ''
            message_stamp = values.stamp
            message_date = values.date
            redis_queryer.zremrangebyscore(setname, message_stamp, message_stamp)
        else
            message_date =  new Date()
            message_stamp =  message_date.getTime()
            values.stamp = message_stamp
            values.date = message_date

        message_body = JSON.stringify(values)
        newMessage = {"body": message_body, "room_id":room_id, "user_id": req.user.id, "name":req.user.name, "date":message_date, stamp:message_stamp}
        m = JSON.stringify newMessage
        redis_publisher.publish("chat #{room_id}", m)
        redis_queryer.zadd(setname,message_stamp, m)
        room_message = support.entity_factory.create('ChatRoomMessage', newMessage)
        room_message.save (err,saved_message) ->
            if !err
                res.send({success:true, newMessage:saved_message})
            else
                next(new Error(err.code,err.message))


module.exports =
    methods: methods,
    build_routes: (app, io) ->
        app.get('/room',methods.get_room)
        app.get('/room/:id', methods.user_authorized_in_room, methods.get_room_by_id)
        app.get('/room/:id/invitations', methods.user_authorized_in_room, methods.get_room_invitations)
        app.get('/room/:id/messages', methods.user_authorized_in_room, socket_middleware(io), methods.get_room_messages)
        app.get('/room/:id/messages/since/:timestamp', methods.user_authorized_in_room, socket_middleware(io), methods.get_room_messages_since_timestamp)
        app.get('/room/:id/users', methods.user_authorized_in_room, methods.manage_room_members)
        app.get('/room/:id/accept-invitation', methods.get_accept_invitation)
        app.post('/room/:id/messages', methods.user_authorized_in_room, express.bodyParser(), methods.post_room_message)
        app.post('/room',express.bodyParser(), methods.post_room)
        app.post('/room/:id/users', methods.user_authorized_in_room, express.bodyParser(), methods.post_room_user)
        app.post('/room/:id/unsubscribe', methods.user_authorized_in_room,  express.bodyParser(), methods.unsubscribe)
