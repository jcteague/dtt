Q = require('q')
node_hash = require('node_hash')
Repository = require('../support/repository')
Validator = require('../support/validator')
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

###
methods.post_registration = (req, res) ->
    validation_result = methods.is_valid_user(req.body)
    if validation_result.valid
        Q.when methods.is_email_already_registered(req.body.email), methods.get_email_registered_handler(req, res)
    else
        res.json {
            success: false
            messages: validation_result.errors
        }
###

methods.get_email_registered_handler = (req, res) ->
    return (is_registered) ->
        if is_registered
            res.json {
                success: false
                messages: ['Email is already registered']
            }
        else
            user_repository = new Repository('User')
            user_data = 
                first_name: trim(req.body.first_name)
                last_name: trim(req.body.last_name)
                email: req.body.email
                password: node_hash.sha256(req.body.password)

            user_repository.save(user_data).then (user) ->
                res.json {
                    success: true
                    messages: ['User created successfully']
                    data: 
                        id: user.id
                        email: user.email
                }

trim = (str) -> str.replace(/^\s\s*/, '').replace(/\s\s*$/, '')

methods.is_valid_user = (user_data) ->
    validator = new Validator()
    validator.check(user_data.first_name, 'First name is invalid').is(/^[A-Za-z]'?[- a-zA-Z]+$/).notEmpty()
    validator.check(user_data.last_name, 'Last name is invalid').is(/^[A-Za-z]'?[- a-zA-Z]+$/).notEmpty()
    validator.check(user_data.email, 'Email is invalid').isEmail().notEmpty()
    validator.check(user_data.password, 'Password must contain at least 6 characters').len(6, 48).notEmpty()
    validator.check(user_data.password, 'Password contains invalid characters').is(/^[A-Za-z0-9\!\@\#\$\%\^\&\*]+$/)

    errors = validator.get_errors()
    return {
        valid: errors.length is 0
        errors: errors
    }

methods.is_email_already_registered = (email) ->
    deferred = Q.defer()
    new Repository('User').find(email: email).then (users) ->
        deferred.resolve(users?)

    deferred.promise

module.exports =
    methods: methods
    build_routes: (app) ->
        app.get('/registration', methods.get_registration)
        app.post('/registration', methods.post_registration)
