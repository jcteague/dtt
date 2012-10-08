express = require('express')
http = require('https');
querystring = require('querystring')
support = require('../support/core').core
Repository = require('../support/repository')
config = require('../config')()
routes_service = require('../support/routes_service')
github_helper = require('../support/github_events_helper')
redis_connector = require('../support/redis/redis_gateway')

build = routes_service.build
redis_publisher = redis_connector.open()
redis_queryer = redis_connector.open()

methods = {}

methods.receive_github_event = (req,res,next) ->
    values = req.body
    new Repository("ChatRoom").find({room_key : req.param('room_key')}).then (rooms) ->
        if(rooms?)
            room = rooms[0]
            setname = "room:#{room.id}:messages"
            notification = github_helper.get_event_message_object values
            
            if notification?
                notification.message = "Github notification!! User, #{notification.user}, just did a #{notification.event_type} on repository: #{notification.repository_name}"
                message_date =  new Date()
                message_stamp =  message_date.getTime()
                notification.date = message_date
                notification.stamp = message_stamp
            
                newMessage = {"body": JSON.stringify(notification), "room_id":room.id, "user_id": room.owner_id, "name":"", "date":message_date, stamp:message_stamp} 
                m = JSON.stringify newMessage
                
                redis_publisher.publish("chat #{room.id}", m)
                redis_queryer.zadd(setname,message_stamp, m)
                room_message = support.entity_factory.create('ChatRoomMessage', newMessage)
                room_message.save (err,saved_message) ->
                    if !err
                        res.send({success:true, newMessage:saved_message})
                    else 
                        next(new Error(err.code,err.message))
        else
            res.send({success:false,messages:["The requested room doesnt exists"]})
            

methods.associate_github_repositories = (req, res, next) ->
    repositories = req.body.repositories
    if repositories?
        owner = req.body.owner
        room_key = req.body.room_key
        response = github_helper.set_github_repository_events(repositories, owner, room_key, req.param("access_token"))
        res.json routes_service.get_server_response(true, ["The webhooks were successfully created"]) # "/room/#{saved_chat_room.id}/" )
    else
        res.json routes_service.get_server_response(false, ["There was an error setting up the webhook"] )
        
methods.github_repositories = (req,res) ->
    callback = (collection) ->
        res.json(collection.to_json())

    build('github_repositories_collection').for({access_token:req.param("access_token"), user_id:req.user.id} ).fetch_to callback

methods.github_authentication_callback = (req, res) ->
    code = req.query.code
    post_fields = 
        'client_id' : config.github.client_id
        'client_secret': config.github.secret
        'code': code
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
            if(typeof(data.access_token) != undefined)
                res.redirect("#{config.site.url}/client#github/repositories/#{data.access_token}")
            else
                res.send({success:false, messages:['There was a problem contacting github']})
        post_res.on 'error', (error) -> 
            console.log("Got error: " + error.message)
            res.send({success:false, messages:[error.message]})
    post_req.end(post_data)


methods.github_redirect = (req, res) ->
    res.redirect("https://github.com/login/oauth/authorize?client_id=#{config.github.client_id}&scope=user,repo&state=#{config.github.state}")

module.exports =
    methods: methods
    build_routes: (app) ->
        app.get('/github/repositories/:access_token', methods.github_repositories)
        app.get('/github/oauth', methods.github_redirect)
        app.get('/github/auth/callback', methods.github_authentication_callback)
        app.post('/github/repositories/:access_token', express.bodyParser(), methods.associate_github_repositories)
        app.post('/github/events/:room_key', express.bodyParser(), methods.receive_github_event)
