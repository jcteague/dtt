express = require('express')
http = require('https')
querystring = require('querystring')
support = require('../../../support/core').core
Repository = require('../../../support/repository')
config = require('../../../config')()
routes_service = require('../../../support/routes_service')
github_helper = require('../../../support/github_events_helper')
redis_connector = require('../../../support/redis/redis_gateway')

build = routes_service.build
redis_publisher = redis_connector.open()
redis_queryer = redis_connector.open()

methods = {}

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

module.exports =
    methods: methods
    build_routes: (app) ->
        app.get('/api/github/repositories/:access_token', methods.github_repositories)
        app.post('/api/github/repositories/:access_token', express.bodyParser(), methods.associate_github_repositories)
