express = require('express')

methods = {}

methods.receive_github_event = (req,res,next) ->
    console.log "OVER HERE"
    values = req.body
    console.log values
    token = req.param("token")
    console.log token
    res.json({success:true})

methods.github_authentication_callback = (req, res, next) ->
    console.log req.params, req.body, req.query

module.exports =
    methods: methods
    build_routes: (app) ->
        app.post('/github/:token', express.bodyParser(), methods.receive_github_event)
        app.get('/github/auth/callback', express.bodyParser(), methods.github_authentication_callback)
