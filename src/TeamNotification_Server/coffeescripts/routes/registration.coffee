build = require('../support/routes_service').build

methods = {}
methods.get_registration = (req, res) ->
    callback = (collection) ->
        res.json(collection.to_json())

    build('registration_collection').fetch_to callback

module.exports =
    methods: methods
    build_routes: (app) ->
        app.get('/registration', methods.get_registration)
