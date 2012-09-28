express = require('express')
support = require('../support/core').core
Repository = require('../repository')

methods = {}

methods.receive_github_event = (req,res,next) ->
    values = req.body
    room_key = req.param("room_key")
#    res.json({success:true})

    chatroom = new Repository('ChatRoom')
    chatroom.find({room_key:room_key}).then (chatrooms) ->
        console.log chatrooms

module.exports =
    methods: methods
    build_routes: (app) ->
        app.post('/github/:room_key', express.bodyParser(), methods.receive_github_event)
