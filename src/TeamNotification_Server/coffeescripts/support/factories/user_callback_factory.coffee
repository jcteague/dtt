node_hash = require('node_hash')
Repository = require('../repository')
user_repository = new Repository('User')

get_for_success = (req, res) ->
    return () ->
        user = sanitize(req.body)
        user_repository.update(user).then (user) ->
            res.json
                success: true
                messages: ['User edited successfully']
                data: 
                    id: user.id
                    email: user.email
                link: "/user/#{user.id}/"

get_for_failure = (req, res) ->
    return (errors) ->
        res.json
            success: false
            messages: errors

sanitize = (user_data) ->
    sanitized = 
        id: user_data.id
        first_name: user_data.first_name.trim()
        last_name: user_data.last_name.trim()
        email: user_data.email.trim()

    sanitized['password'] = node_hash.sha256(user_data.password) if user_data.password isnt ''
    sanitized

module.exports =
    get_for_success: get_for_success
    get_for_failure: get_for_failure
