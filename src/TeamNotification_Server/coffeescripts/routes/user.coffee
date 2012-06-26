build = require('../support/routes_service').build
methods = {}

methods.get_user = (req, res) ->
    user_id = req.param('id')
    callback = (collection) ->
        res.json(collection.to_json())

    build('user_collection').for(user_id).fetch_to callback

methods.get_user_rooms = (req, res) ->
    user_id = req.param('id')
    callback = (collection) ->
        res.json(collection.to_json())

    build('user_rooms_collection').for(user_id).fetch_to callback

methods.get_users = (req, res)->
    username = req.param('q')  
    console.log username
    callback = (collection) ->
        res.json(collection.to_json())
    build('users_collection').for(username).fetch_to callback

module.exports =
    methods: methods
    build_routes: (app) ->
        app.get('/users', methods.get_users)
        app.get('/users/:username?', methods.get_users)
        app.get('/user/:id',methods.get_user)
        app.get('/user/:id/rooms',methods.get_user_rooms)
