passport = require('passport')
build = require('../../../support/routes_service').build

methods = {}
methods.get_root = (req, res) ->
    callback = (collection) ->
        res.json(collection.to_json())

    #build('root_collection').for(req.user.id).fetch_to callback
    build('root_collection').for().fetch_to callback

module.exports =
    methods: methods,
    build_routes: (app, io) ->
        app.get('/api',methods.get_root)
