express = require('express')
support = require('../support/core').core
Repository = require('../support/repository')
config = require('../config')()
querystring = require('querystring')
http = require('https');
routes_service = require('../support/routes_service')
build = routes_service.build
github_helper = require('../support/github_events_helper')
methods = {}

methods.receive_github_event = (req,res,next) ->
    values = req.body
    console.log values
#    room_key = req.param("room_key")
    #chat_room = new Repository('ChatRoom')
    #chat_room.find({room_key:room_key}).then (chat_rooms) ->
    #    console.log chat_rooms

methods.associate_github_repositories = (req, res, next) ->
    repositories = req.body.repositories
    if repositories?
        owner = req.body.owner
        room_key = req.body.room_key
        res.json(github_helper.set_github_repository_events(repositories, owner, room_key, req.param("access_token")))
    else
        res.json({success:false,messages:['There was an error'] })
        
methods.github_repositories = (req,res) ->
    callback = (collection) ->
        res.json(collection.to_json())

    build('github_repositories_collection').for({access_token:req.param("access_token"), user_id:req.user.id} ).fetch_to callback

methods.github_authentication_callback = (req, res) ->
    code = req.query.code
    console.log code
    post_fields = 
        'client_id' : config.github.client_id
        'client_secret': config.github.secret
        'code': req.query.code
        'state': config.github.state
    post_data = querystring.stringify( post_fields)
    post_options = 
        host: 'github.com'
        port: 443
        method: 'POST'
        path: '/login/oauth/access_token'
        headers:
            'Accept': 'application/json'
            'Content-Type': 'application/x-www-form-urlencoded'
            'Content-Length': post_data.length
    post_req = http.request post_options, (post_res) ->        
        post_res.setEncoding('utf8')
        post_res.on 'data', (chunk) ->
            data = JSON.parse(chunk)
            console.log data
            res.redirect("#{config.site.url}/client#github/repositories/#{data.access_token}")
        post_res.on 'error', (e) -> 
            console.log("Got error: " + e.message)
    post_req.end(post_data)


methods.github_redirect = (req, res) ->
    res.redirect("https://github.com/login/oauth/authorize?client_id=#{config.github.client_id}&scope=user,repo&state=#{config.github.state}")

module.exports =
    methods: methods
    build_routes: (app) ->
        app.get('/github/repositories/:access_token', methods.github_repositories)
        app.get('/github/oauth', methods.github_redirect)
        app.get('/github/auth/callback', methods.github_authentication_callback)
        app.post('/github/repositories/:access_token', methods.associate_github_repositories)
        app.post('/github/:room_key', express.bodyParser(), methods.receive_github_event)
