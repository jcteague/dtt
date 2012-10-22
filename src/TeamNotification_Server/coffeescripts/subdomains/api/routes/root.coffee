passport = require('passport')
build = require('../../../support/routes_service').build

methods = {}
methods.get_root = (req, res) ->
    callback = (collection) ->
        res.json(collection.to_json())

    build('root_collection').fetch_to callback

module.exports =
    methods: methods,
    build_routes: (app, io) ->
        app.get('/',methods.get_root)
