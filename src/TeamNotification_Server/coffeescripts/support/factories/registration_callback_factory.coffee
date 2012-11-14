node_hash = require('node_hash')
registration_service = require('../registration_service')
email_sender = require('../email/email_sender')
email_template = require('../email/templates/email_template')

get_for_success = (req, res) ->
    return () ->
        user_data =
            first_name: req.body.first_name.trim()
            last_name: req.body.last_name.trim()
            email: req.body.email.trim()
            password: node_hash.sha256(req.body.password)
            enabled: 0

        registration_service.create_user(user_data).then (user) ->
            console.log 'New USer'
            console.log user
            template = email_template.for.confirmation.using
                    email: user.email
                    name: user.first_name + ' ' + user.last_name
                    confirmation_key: user.confirmation_key
            email_sender.send template
            res.json {
                success: true
                messages: ['User created successfully']
                data:
                    id: user.id
                    email: user.email
                link: "/user/#{user.id}/"
            }
            

get_for_failure = (req, res) ->
    return (errors) ->
        res.json
            success: false
            messages: errors

module.exports =
    get_for_success: get_for_success
    get_for_failure: get_for_failure
