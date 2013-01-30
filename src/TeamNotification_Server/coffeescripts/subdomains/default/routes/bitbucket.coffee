express = require('express')
http = require('https')
querystring = require('querystring')
support = require('../../../support/core').core
Repository = require('../../../support/repository')
config = require('../../../config')()
routes_service = require('../../../support/routes_service')
bitbucket_helper = require('../../../support/bitbucket_events_helper')
add_user_data_to_collection = require('../../../support/routes_service').add_user_data_to_collection
get_oauth_client = require('../../../support/oauth_client_provider').bitbucket_oauth_client
build = routes_service.build
redis_connector = require('../../../support/redis/redis_gateway')
redis_publisher = redis_connector.open()
redis_queryer = redis_connector.open()

methods = {}
methods.associate_bitbucket_repositories = (req, res, next) ->
    repositories = req.body.repositories
    if repositories?
        owner = req.body.owner
        room_key = req.body.room_key
        response = bitbucket_helper.set_bitbucket_repository_events(repositories, room_key, req.param("oauth_token"), req.query.oauth_token_secret)
        res.json routes_service.get_server_response(true, ["The webhooks were successfully created"])
    else
        res.json routes_service.get_server_response(false, ["There was an error setting up the webhook"] )

methods.bitbucket_repositories = (req,res)->
    oa = get_oauth_client()
    callback = (collection) ->
        add_user_data_to_collection(req.user, collection.to_json()).then (json) ->
            res.json(json)
    build('bitbucket_repositories_collection').for({oauth_access_token:req.param("oauth_token"), oauth_access_token_secret:req.query.oauth_access_token_secret, user_id:req.user.id} ).fetch_to callback


methods.receive_bitbucket_event = (req,res,next)->
    console.log "Bitbucket event"
    console.log req.params
    
    values = req.body
    console.log values
    new Repository("ChatRoom").find({room_key : req.param('room_key')}).then (rooms) ->
        if(rooms?)
            room = rooms[0]
            setname = "room:#{room.id}:messages"
            
            event_obj = JSON.parse(values.payload)
            message_date = new Date()            
            notification = 
                user:event_obj.user
                event_type:'push'
                repository_name:event_obj.repository.name
                repository_url:"https://bitbucket.org/#{event_obj.repository.absolute_url}"
                content:''
                message: "Bitbucket notification! User, #{event_obj.user}, just did a push on repository: #{event_obj.repository.name} {0} - {1}"
                notification:1
                source:'Bitbucket notification'
                url: ''
                date: message_date,
                stamp: message_date.getTime()
            console.log notification
            if notification?
                newMessage = {"body": JSON.stringify(notification), "room_id":room.id, "user_id": -1, "name":"bitbucket", "date":notification.date, stamp:notification.stamp}
                m = JSON.stringify newMessage

                redis_publisher.publish("/api/room/#{room.id}/messages", m)
                redis_queryer.zadd(setname,notification.stamp, m)
                room_message = support.entity_factory.create('ChatRoomMessage', newMessage)
                room_message.save (err,saved_message) ->
                    if !err
                        res.send({success:true, newMessage:saved_message})
                    else
                        next(new Error(err.code,err.message))
        else
            res.send({success:false,messages:["The requested room does not exist"]})


methods.bitbucket_authentication_callback = (req, res)->
    oa = get_oauth_client()
    oa.getOAuthAccessToken req.session.oauth_token, req.session.oauth_token_secret, req.query.oauth_verifier, (error, oauth_access_token, oauth_access_token_secret, results)->
        if error? 
            console.log('error')
            console.log(error)
            res.send({success:false, messages:['There was a problem contacting bitbucket']})
        else
            req.session.oauth_access_token = oauth_access_token
            req.session.oauth_access_token_secret = oauth_access_token_secret
            res.redirect "#{config.site.surl}/#/bitbucket/repositories/#{oauth_access_token}?oauth_access_token_secret=#{oauth_access_token_secret}"
                    
methods.bitbucket_redirect = (req, res)->
    oa = get_oauth_client()
    oa.getOAuthRequestToken (error, oauth_token, oauth_token_secret, results)->
        if error?
            console.log error
            res.send({success:false, messages:['There was a problem contacting bitbucket']})
        else
            req.session.oa = oa
            req.session.oauth_token = oauth_token
            req.session.oauth_token_secret = oauth_token_secret
            res.redirect "https://bitbucket.org/!api/1.0/oauth/authenticate?oauth_token=#{oauth_token}&oauth_token_secret=#{oauth_token_secret}"

module.exports =
    methods: methods
    build_routes: (app) ->
        app.get('/api/bitbucket/repositories/:oauth_token', methods.bitbucket_repositories)
        app.post('/api/bitbucket/repositories/:oauth_token', express.bodyParser(), methods.associate_bitbucket_repositories)
        app.get('/bitbucket/oauth/callback', methods.bitbucket_authentication_callback)
        app.get('/bitbucket/oauth', methods.bitbucket_redirect)
        app.post('/bitbucket/events/:room_key', express.bodyParser(), methods.receive_bitbucket_event )
