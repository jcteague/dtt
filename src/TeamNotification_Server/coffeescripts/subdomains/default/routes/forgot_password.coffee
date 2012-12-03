methods = {}
sha256 = require('node_hash').sha256
config = require('../../../config')()
Repository = require('../../../support/repository')
get_server_response = require('../../../support/routes_service').get_server_response
express = require('express')
email_sender = require('../../../support/email/email_sender')
email_template = require('../../../support/email/templates/email_template')
build = require('../../../support/routes_service').build


methods.forgot_password_form = (req, res) ->
    r =
        'href': '/forgot_password'
        'links' : [
          {"name": "self", "rel": "login", 'href':'/forgot_password'}
        ]
        'template':
            'data':[
                {'name':'email', 'label':'Email', 'type':'string'}
            ]
    res.json(r)

methods.send_reset_email = (req, res, next) ->
    email = req.body.email
    user = new Repository('User')
    user.find(email:email, enabled:1).then (users) ->
        if users? && users[0]?
            d = new Date().getTime().toString()
            reset_key = sha256(users[0].email+d)
            
            newPasswordResetRequest =
                user_id: users[0].id
                reset_key:reset_key
                
            user_password_reset_request_repository = new Repository('UserPasswordResetRequest')
            user_password_reset_request_repository.save(newPasswordResetRequest).then (saved_password_reset_request) ->
                template = email_template.for.password_reset.using
                    email: users[0].email
                    reset_key: saved_password_reset_request.reset_key
                email_sender.send template
                r = get_server_response(true, ['An email has been sent with a link to reset your password.'])
                res.json(r)
            
        else
            r = get_server_response(false, ["There's no user with the provided email."])
            res.json(r)
        
methods.reset_form = (req, res) ->
    callback = (collection) ->
        res.json(collection.to_json())
    user_password_reset_request_repository = new Repository('UserPasswordResetRequest')
    user_password_reset_request_repository.find(reset_key:req.param('reset_key'), active:1).then ( user_password_reset_request) ->
        if(user_password_reset_request? && user_password_reset_request[0]?)
            build('change_password_collection').for(reset_key:req.param('reset_key')).fetch_to callback
        else
            res.redirect(config.site.surl + '/api/user/login', 301)

methods.reset_password = (req, res, next) ->
    user_password_reset_request_repository = new Repository('UserPasswordResetRequest')
    values = req.body
    
    if(values.password != values.confirm_password)
        res.json get_server_response(false, ["Both fields must match"])
    
    user_password_reset_request_repository.find(reset_key:req.param('reset_key'), active:1).then ( user_password_reset_request) ->
        if(user_password_reset_request? && user_password_reset_request[0]?)
            user_password_reset_request[0].user.password = sha256(values.password)
            user_password_reset_request[0].user.save (err, saved_user) ->
                if !err
                    user_password_reset_request[0].active = 0
                    user_password_reset_request[0].save()
                    r = get_server_response(true, ["User password has been updated successfully"])
                    res.json(r)
                else
                    r = get_server_response(false, ["There has been an error in our servers"])
                    res.json(r)
        else
            r = get_server_response(false, ["The provided reset key is invalid"])
            res.json(r)
            
module.exports = 
    methods: methods
    build_routes: (app) ->
        app.get('/api/forgot_password', methods.forgot_password_form )
        app.post('/api/forgot_password', express.bodyParser(), methods.send_reset_email )
        app.get('/api/reset_password/:reset_key', methods.reset_form)
        app.post('/api/reset_password/:reset_key', express.bodyParser(), methods.reset_password)
