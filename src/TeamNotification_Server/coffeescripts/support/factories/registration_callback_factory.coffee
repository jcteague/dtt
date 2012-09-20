node_hash = require('node_hash')
registration_service = require('../registration_service')

get_for_success = (req, res) ->
    return () ->
        user_data = 
            first_name: trim(req.body.first_name)
            last_name: trim(req.body.last_name)
            email: req.body.email
            password: node_hash.sha256(req.body.password)

        registration_service.create_user(user_data).then (user) ->
            res.json {
                success: true
                messages: ['User created successfully']
                data: 
                    id: user.id
                    email: user.email
            }

get_for_failure = (req, res) ->
    return (errors) ->
        res.json
            success: false
            messages: errors

trim = (str) -> str.replace(/^\s\s*/, '').replace(/\s\s*$/, '')

module.exports =
    get_for_success: get_for_success
    get_for_failure: get_for_failure
