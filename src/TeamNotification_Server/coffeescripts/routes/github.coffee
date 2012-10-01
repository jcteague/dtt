express = require('express')
support = require('../support/core').core
Repository = require('../support/repository')
config = require('../config')()
querystring = require('querystring')
http = require('https');

methods = {}

methods.receive_github_event = (req,res,next) ->
    values = req.body
    room_key = req.param("room_key")
    chat_room = new Repository('ChatRoom')
    chat_room.find({room_key:room_key}).then (chat_rooms) ->
        console.log chat_rooms

methods.github_authentication_callback = (req, res) ->
    code = req.query.code
    console.log 'step 1: getting the code'
    post_fields = 
        'client_id' : config.github.client_id
        'client_secret': config.github.secret
        'code': req.query.code
        'state': config.github.state
    post_data = querystring.stringify( post_fields)
    
    console.log 'step 2: setting the post_data'
    
    post_options = 
        host: 'github.com'
        port: 443
        method: 'POST'
        path: '/login/oauth/access_token'
        headers:
            'Accept': 'application/json'
            'Content-Type': 'application/x-www-form-urlencoded'
            'Content-Length': post_data.length
            
    console.log 'step 3: setting the post_option'
    
    post_req = http.request post_options, (post_res) ->        
        post_res.setEncoding('utf8')
        post_res.on 'data', (chunk) ->
            res.send(success:true, data: chunk)
        post_res.on 'error', (e) -> 
            console.log("Got error: " + e.message)
            
    console.log 'step 4:setting the response callback'
    post_req.end(post_data)
    console.log 'Finish'


methods.github_redirect = (req, res) ->
    console.log 'Redirecting'
    res.redirect("https://github.com/login/oauth/authorize?client_id=#{config.github.client_id}&scope=user,repo&state=#{config.github.state}")

module.exports =
    methods: methods
    build_routes: (app) ->
        app.post('/github/:room_key', express.bodyParser(), methods.receive_github_event)
        app.get('/github/oauth', methods.github_redirect)
        app.get('/github/auth/callback', methods.github_authentication_callback)
