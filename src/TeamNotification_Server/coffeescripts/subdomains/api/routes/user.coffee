methods = {}
build = require('../../../support/routes_service').build
user_validator = require('../../../support/validation/user_validator')
user_callback_factory = require('../../../support/factories/user_callback_factory')
routes_service = require('../../../support/routes_service')
get_server_response = routes_service.get_server_response

express = require('express')
sha256 = require('node_hash').sha256
config = require('../../../config')()
Repository = require('../../../support/repository')

methods.get_user = (req, res) ->
    get_user_collection(req, res, req.param('id'))

methods.get_logged_user = (req, res) ->
    get_user_collection(req, res, req.user.id)

get_user_collection = (req, res, user_id) ->
    callback = (collection) ->
        res.json(collection.to_json())

    build('user_collection').for(user_id).fetch_to callback

methods.get_user_edit = (req, res) ->
    user_id = req.param('id')
    callback = (collection) ->
        res.json(collection.to_json())

    build('user_edit_collection').for(user_id).fetch_to callback

methods.get_user_invitations = (req, res) ->
    
    callback = (collection) ->
        res.json(collection.to_json())

    build('user_invitations_collection').for(req.user.id).fetch_to callback

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

methods.confirm = (req, res) ->
    confirmation_key = req.param('confirmation_key')
    user_id = req.param('id')
    user_confirmation_keys = new Repository('UserConfirmationKey')
    user_confirmation_keys.find(confirmation_key:confirmation_key).then (user_confirmation_keys) ->
        if(!user_confirmation_keys && !user_confirmation_keys[0])
            res.json(get_server_response(false, ["Confirmation key is not correct"], "/user/login"))
        user_confirmation_key = user_confirmation_keys[0]
        user_confirmation_key.active = 0
        user_confirmation_key.user.enabled = 1
        
        user_confirmation_key.user.save (err, saved_user) ->
            if !err
                user_confirmation_key.save (err2, saved_uck) -> 
                    if(!err2)
                        res.json(get_server_response(true, ["User has been confirmed"], "/user/login"))
                    else
                        res.json(get_server_response(false, ["User confirmation was unsuccessful correct"], "/user/login"))
            else
                res.json(get_server_response(false, ["User confirmation was unsuccessful correct"], "/user/login"))
            
    #callback = (collection) ->
    #    res.json(collection.to_json())
    
    #build('user_confirm_collection').for(user_id, confirm_key).fetch_to callback

methods.authenticate = (req, res, next) ->
    values = req.body
    email = values.username
    pass = sha256(values.password)
    callback = (collection) ->
        user_data = collection.to_json()
        if JSON.stringify( user_data ) != '{}'
            auth_token = "Basic " + (new Buffer(email + ":" + values.password).toString('base64'))
            #res.send({success: true, redis:config.redis, user:{id: user_data.id, email:user_data.email, authtoken:req.headers.authorization}})
            res.send({success: true, redis:config.redis, user:{id: user_data.id, email:user_data.email, authtoken:auth_token}})
        else
            res.send({})
          
    build('user_login_collection').for(email:email, password:pass).fetch_to callback
    
module.exports = 
    methods: methods
    build_routes: (app) ->
        app.get('/user/invitations', methods.get_user_invitations)
        app.get('/users/query', methods.get_users)
        app.get('/user/login', methods.login)
        app.post('/user/login',express.bodyParser(), methods.authenticate)
        app.get('/user/confirm/:confirmation_key', methods.confirm)
        app.get('/user', methods.get_logged_user)
        app.get('/user/:id', methods.get_user)
        app.get('/user/:id/edit', methods.get_user_edit)
        app.post('/user/:id/edit', methods.post_user_edit)
        app.get('/user/:id/rooms',methods.get_user_rooms)
        app.get('/users', methods.redir_user)
