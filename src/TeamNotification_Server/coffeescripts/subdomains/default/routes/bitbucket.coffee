express = require('express')
http = require('https')
querystring = require('querystring')
support = require('../../../support/core').core
Repository = require('../../../support/repository')
config = require('../../../config')()
routes_service = require('../../../support/routes_service')
#bitbucket_helper = require('../../../support/bitbucket_events_helper')
add_user_data_to_collection = require('../../../support/routes_service').add_user_data_to_collection
OAuth= require('oauth').OAuth
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
        #response = bitbucket_helper.set_bitbucket_repository_events(repositories, owner, room_key, req.param("access_token"))
        res.json routes_service.get_server_response(true, ["The webhooks were successfully created"])
    else
        res.json routes_service.get_server_response(false, ["There was an error setting up the webhook"] )

methods.bitbucket_repositories = (req,res)->
    oa = new OAuth req.session.oa._requestUrl,
               req.session.oa._accessUrl,
               req.session.oa._consumerKey,
               req.session.oa._consumerSecret,
               req.session.oa._version,
               req.session.oa._authorize_callback,
               req.session.oa._signatureMethod
    callback = (collection) ->
        add_user_data_to_collection(req.user, collection.to_json()).then (json) ->
            res.json(json)
    build('bitbucket_repositories_collection').for({oa:oa, oauth_access_token:req.param("access_token"), oauth_access_token_secret:req.session.oauth_access_token_secret, user_id:req.user.id} ).fetch_to callback


methods.receive_bitbucket_event = (req,res,next)->
    console.log "Bitbucket event"
    console.log req.params

methods.bitbucket_authentication_callback = (req, res)->
    oauth_verifier = req.query.oauth_verifier
    oa = new OAuth req.session.oa._requestUrl,
                   req.session.oa._accessUrl,
                   req.session.oa._consumerKey,
                   req.session.oa._consumerSecret,
                   req.session.oa._version,
                   req.session.oa._authorize_callback,
                   req.session.oa._signatureMethod
   
    oa.getOAuthAccessToken req.session.oauth_token, req.session.oauth_token_secret, req.query.oauth_verifier, (error, oauth_access_token, oauth_access_token_secret, results)->
			    if error? 
                    console.log('error')
                    console.log(error)
                    res.send({success:false, messages:['There was a problem contacting bitbucket']})
	     		else
                    req.session.oauth_access_token = oauth_access_token
                    req.session.oauth_access_token_secret = oauth_access_token_secret
                    res.redirect "#{config.site.surl}/#/bitbucket/repositories/#{oauth_access_token}"
                    
methods.bitbucket_redirect = (req, res)->
    oa= new OAuth "https://bitbucket.org/!api/1.0/oauth/request_token",
                  "https://bitbucket.org/!api/1.0/oauth/access_token",
                  config.bitbucket.key,#"Mqe9u2gsrbMchaZrzB",
                  config.bitbucket.secret,# "kT2zTu8zhNsdCj3jgSrhVPCnyufRvmu9",
                  "1.0",
                  "#{config.site.surl}/bitbucket/oauth/callback",
                  "HMAC-SHA1"
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
        app.get('/api/bitbucket/repositories/:access_token', methods.bitbucket_repositories)
        app.post('/api/bitbucket/repositories/:access_token', express.bodyParser(), methods.associate_bitbucket_repositories)
        app.get('/bitbucket/oauth/callback', methods.bitbucket_authentication_callback)
        app.get('/bitbucket/oauth', methods.bitbucket_redirect)
