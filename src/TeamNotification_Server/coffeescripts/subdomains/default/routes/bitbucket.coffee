express = require('express')
http = require('https')
querystring = require('querystring')
support = require('../../../support/core').core
Repository = require('../../../support/repository')
config = require('../../../config')()
routes_service = require('../../../support/routes_service')
github_helper = require('../../../support/github_events_helper')
redis_connector = require('../../../support/redis/redis_gateway')
add_user_data_to_collection = require('../../../support/routes_service').add_user_data_to_collection
OAuth= require('oauth').OAuth
build = routes_service.build
redis_publisher = redis_connector.open()
redis_queryer = redis_connector.open()

methods = {}
methods.bitbucket_authentication_callback = (req, res)->
    oauth_verifier = req.query.oauth_verifier
    console.log req.session.oa
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
	     		else
	     		    req.session.oauth_access_token = oauth_access_token
				    req.session.oauth_access_token_secret = oauth_access_token_secret
                    res.redirect("#{config.site.surl}/api/bitbucket/repositories/:access_token"
                    
methods.bitbucket_redirect = (req, res)->
    oa= new OAuth "https://bitbucket.org/!api/1.0/oauth/request_token",
                  "https://bitbucket.org/!api/1.0/oauth/access_token",
                  "Mqe9u2gsrbMchaZrzB",
                  "kT2zTu8zhNsdCj3jgSrhVPCnyufRvmu9",
                  "1.0",
                  "#{config.site.surl}/bitbucket/oauth/callback",
                  "HMAC-SHA1"
    oa.getOAuthRequestToken (error, oauth_token, oauth_token_secret, results)->
        if error?
            console.log error
            res.send({success:false, messages:['There was a problem contacting bitbucket']})
        else
            console.log results
            req.session.oa = oa
            req.session.oauth_token = oauth_token
            req.session.oauth_token_secret = oauth_token_secret
            res.redirect "https://bitbucket.org/!api/1.0/oauth/authenticate?oauth_token=#{oauth_token}"

methods.bitbucket_repositories = (req, res)->
    res.json(
    #oa.getOAuthRequestToken (error, oauth_token, oauth_token_secret, results)->
           
module.exports =
    methods: methods
    build_routes: (app) ->
        app.get('/api/bitbucket/repositories/:access_token', methods.bitbucket_repositories)
        app.post('/api/bitbucket/repositories/:access_token', express.bodyParser(), methods.associate_bitbucket_repositories)
        app.get('/bitbucket/oauth/callback', methods.bitbucket_authentication_callback)
        app.get('/bitbucket/oauth', methods.bitbucket_redirect)
        #app.get('/api/bitbucket/repositories/:access_token', methods.bitbucket_repositories)
