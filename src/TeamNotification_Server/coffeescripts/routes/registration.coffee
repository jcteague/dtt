build = require('../support/routes_service').build
registration_validator = require('../support/validation/registration_validator')
registration_callback_factory = require('../support/factories/registration_callback_factory')

methods = {}
methods.get_registration = (req, res) ->
    callback = (collection) ->
        res.json(collection.to_json())

    build('registration_collection').fetch_to callback

methods.post_registration = (req, res) ->
    success_callback = registration_callback_factory.get_for_success(req, res)
    failure_callback = registration_callback_factory.get_for_failure(req, res)
    registration_validator.validate(req.body).handle_with success_callback, failure_callback

module.exports =
    methods: methods
    build_routes: (app) ->
        app.get('/registration', methods.get_registration)
        app.post('/registration', methods.post_registration)
