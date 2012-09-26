express = require('express')

methods = {}

methods.receive_github_event = (req,res,next) ->
    console.log "OVER HERE"
    values = req.body
    console.log values
    token = req.param("token")
    console.log token
    res.json({success:true})

module.exports =
    methods: methods
    build_routes: (app) ->
        app.post('/github/:token', express.bodyParser(), methods.receive_github_event)
