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

get_for_failure = (req, res) ->
    return (errors) ->
        res.json
            success: false
            messages: errors

sanitize = (user_data) ->
    return {
        id: user_data.id
        first_name: trim(user_data.first_name)
        last_name: trim(user_data.last_name)
        email: user_data.email
        password: node_hash.sha256(user_data.password)
    }

trim = (str) -> str.replace(/^\s\s*/, '').replace(/\s\s*$/, '')

module.exports =
    get_for_success: get_for_success
    get_for_failure: get_for_failure
