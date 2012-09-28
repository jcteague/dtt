express = require('express')
support = require('../support/core').core
Repository = require('../support/repository')

methods = {}

methods.receive_github_event = (req,res,next) ->
    values = req.body
    room_key = req.param("room_key")
    chat_room = new Repository('ChatRoom')
    chat_room.find({room_key:room_key}).then (chat_rooms) ->
        console.log chat_rooms

module.exports =
    methods: methods
    build_routes: (app) ->
        app.post('/github/:room_key', express.bodyParser(), methods.receive_github_event)
