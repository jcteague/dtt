build = require('../../../support/routes_service').build

methods = {}
methods.get_plugin_version = (req, res) ->
    callback = (collection) ->
        res.json(collection.to_json())

    build('plugin_collection').fetch_to callback

methods.get_plugin_download = (req, res) ->


module.exports =
    methods: methods,
    build_routes: (app, io) ->
        app.get('/plugin', methods.get_plugin_version)
        app.get('/plugin/download/TeamNotification_Package.vsix', methods.get_plugin_download)
