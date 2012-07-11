node_hash = require('node_hash')
Repository = require('../support/repository')
Validator = require('../support/validator')
build = require('../support/routes_service').build

methods = {}
methods.get_registration = (req, res) ->
    callback = (collection) ->
        res.json(collection.to_json())

    build('registration_collection').fetch_to callback

methods.post_registration = (req, res) ->
    user_repository = new Repository('User')
    if methods.is_valid_user(req.body)
        user_data = 
            first_name: req.body.first_name
            last_name: req.body.last_name
            email: req.body.email
            password: node_hash.sha256(req.body.password)

        user_repository.save(user_data).then (user) ->
            res.json 
                success: true
                data: user
    else
        res.json
            success: false
            message: 'user data is invalid'

methods.is_valid_user = (user_data) ->
    validator = new Validator()
    validator.check(user_data.first_name, 'Invalid First Name').isAlphanumeric()
    validator.check(user_data.first_name, 'Invalid First Name').notEmpty()
    validator.check(user_data.last_name, 'Invalid Last Name').isAlphanumeric()
    validator.check(user_data.last_name, 'Invalid Last Name').notEmpty()
    validator.check(user_data.email, 'Invalid Email').isEmail()
    validator.check(user_data.email, 'Invalid Email').notEmpty()
    validator.check(user_data.password, 'Invalid Password').len(6, 48)
    validator.check(user_data.password, 'Invalid Password').is(/^[A-Za-z0-9\!\@\#\$\%\^\&\*]+$/)
    validator.check(user_data.password, 'Invalid Password').notEmpty()

    errors = validator.get_errors()
    console.log errors
    return {
        valid: errors.length is 0
        errors: errors
    }

module.exports =
    methods: methods
    build_routes: (app) ->
        app.get('/registration', methods.get_registration)
        app.post('/registration', methods.post_registration)
