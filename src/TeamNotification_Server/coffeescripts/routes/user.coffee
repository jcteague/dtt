methods = {}
build = require('../support/routes_service').build
user_validator = require('../support/validation/user_validator')
user_callback_factory = require('../support/factories/user_callback_factory')

express = require('express')
sha256 = require('node_hash').sha256
config = require('../config')()

methods.get_user = (req, res) ->
    user_id = req.param('id')
    callback = (collection) ->
        res.json(collection.to_json())

    build('user_collection').for(user_id).fetch_to callback

methods.get_user_edit = (req, res) ->
    user_id = req.param('id')
    callback = (collection) ->
        res.json(collection.to_json())

    build('user_edit_collection').for(user_id).fetch_to callback

methods.post_user_edit = (req, res) ->
    success_callback = user_callback_factory.get_for_success(req, res)
    failure_callback = user_callback_factory.get_for_failure(req, res)
    user_validator.validate(req.body).handle_with success_callback, failure_callback

methods.get_user_rooms = (req, res) ->
    user_id = req.param('id')
    callback = (collection) ->
        res.json(collection.to_json())

    build('user_rooms_collection').for(user_id).fetch_to callback

methods.get_users = (req, res)->
    username = req.param('q')  
    callback = (collection) ->
        res.json(collection.to_json())
    build('users_collection').for(username).fetch_to callback
    
methods.redir_user = (req, res) ->
    user_id = req.user.id
    res.redirect("#{config.site.url}/user/#{user_id}")
    
methods.login = (req, res) ->
    r =
        'href': '/user/login'
        'links' : [
          {"name": "self", "rel": "login", 'href':'/user/login'}
        ]
        'template':
            'data':[
                {'name':'username', 'label':'Username', 'type':'string'}
                {'name':'password', 'label':'Password', 'type':'password'}
            ]
    res.json(r)

methods.authenticate = (req, res, next) ->
    values = req.body
    email = values.username
    pass = sha256(values.password)
    callback = (collection) ->
        user_data = collection.to_json()
        if JSON.stringify( user_data ) != '{}'
            res.send({success: true, redis:config.redis, user:{id: user_data.id, email:user_data.email, authtoken:req.headers.authorization}})
        else
            res.send({})
          
    build('user_login_collection').for(email:email, password:pass).fetch_to callback
    
module.exports = 
    methods: methods
    build_routes: (app) ->
        app.get('/users/query', methods.get_users)
        app.get('/user/login', methods.login)
        app.post('/user/login',express.bodyParser(), methods.authenticate)
        app.get('/user/:id', methods.get_user)
        app.get('/user/:id/edit', methods.get_user_edit)
        app.post('/user/:id/edit', methods.post_user_edit)
        app.get('/user/:id/rooms',methods.get_user_rooms)
        app.get('/users', methods.redir_user)
