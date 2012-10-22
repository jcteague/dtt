Q = require('q')
Repository = require('../repository')
Validator = require('../validator')

user_repository = new Repository('User')

validate = (user_data) ->
    return {
        handle_with: (success_callback, failure_callback) ->
            Q.when is_valid_user(user_data), (validation_result) ->
                if validation_result.valid
                    success_callback()
                else
                    failure_callback(validation_result.errors)
    }

is_valid_user = (user_data) ->
    Q.fcall(() ->
        is_valid_user_data(user_data)
    ).then((validation_result) ->
        if validation_result.valid
            user_repository.find(email: user_data.email).then (users) ->
                if users?
                    return {
                        valid: false
                        errors: ['Email is already registered']
                    }
                else
                    return valid: true
        else
            validation_result
    )

is_valid_user_data = (user_data) ->
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


module.exports =
    validate: validate
