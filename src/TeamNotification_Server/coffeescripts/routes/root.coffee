passport = require('passport')
build = require('../support/routes_service').build

methods = {}
methods.get_root = (req, res) ->
    callback = (collection) ->
        res.json(collection.to_json())

    build('root_collection').fetch_to callback

module.exports =
    methods: methods,
    build_routes: (app) ->
        app.get('/', passport.authenticate('basic', session: false), methods.get_root)
