build = require('../../../support/routes_service').build

methods = {}
methods.get_plugin_version = (req, res) ->
    callback = (collection) ->
        res.json(collection.to_json())

    build('plugin_collection').fetch_to callback

module.exports =
    methods: methods,
    build_routes: (app, io) ->
        app.get('/plugin/version', methods.get_plugin_version)
