express = require('express')
support = require('../support/core').core
Repository = require('../support/repository')
config = require('../config')()
querystring = require('querystring')
http = require('http');

methods = {}

methods.receive_github_event = (req,res,next) ->
    values = req.body
    room_key = req.param("room_key")
    chat_room = new Repository('ChatRoom')
    chat_room.find({room_key:room_key}).then (chat_rooms) ->
        console.log chat_rooms

methods.github_authentication_callback = (req, res) ->
    code = req.query.code
    post_fields = 
        'client_id' : config.github.client_id
        'client_secret': config.github.secret
        'code': req.query.code
        'state': config.github.state
    post_data = querystring.stringify( post_fields)
    
    post_options = 
        host: 'https://github.com/login/oauth/access_token'
        method: 'POST'
        headers:
            'Accept': 'application/json'
            'Content-Type': 'application/x-www-form-urlencoded'
            'Content-Length': post_data.length
            
    post_req = http.request post_options, (post_res) ->
        post_res.setEncoding('utf8')
        post_res.on 'data', (chunk) ->
            console.log 'Response: ' + chunk
    post_req.write(post_data);
    post_req.end();
            
methods.github_redirect = (req, res) ->
    res.redirect("https://github.com/login/oauth/authorize?client_id=#{config.github.client_id}&scope=user,repo&redirect_uri=#{config.github.redirect_url}&state=#{config.github.state}")

module.exports =
    methods: methods
    build_routes: (app) ->
        app.post('/github/:room_key', express.bodyParser(), methods.receive_github_event)
        app.get('/github/oauth', methods.github_redirect)
        app.get('/github/auth/callback', methods.github_authentication_callback)
