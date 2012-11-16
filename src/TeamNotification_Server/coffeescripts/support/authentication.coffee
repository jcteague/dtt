_ = require('underscore')
passport = require('passport')
sha256 = require('node_hash').sha256
BasicStrategy = require('passport-http').BasicStrategy
Repository = require './repository'
whitelisted_paths = require('../config')().site.whitelisted_paths

class Authentication

	constructor: (basic_strategy = null) ->
        _.bindAll @
        @repository = new Repository('User')
        @basic_strategy = if basic_strategy? then basic_strategy else new BasicStrategy({},@findByUserName)
        passport.use(@basic_strategy)

    initializeAuth: ->
        passport.initialize()

    findByUserName: (username,password,done) ->
        @repository.find(email: username, password: sha256(password), enabled: 1).then (users) ->
            if !users? or !users[0]?
                return done(null, false)
            user = users[0]
            done(null, id: user.id, email: user.email, name: user.first_name, enabled:user.enabled)

	authenticate: (request, response, next) ->
        if @is_whitelisted(request.path)
            next()
        else
            if typeof request.cookies != 'undefined' && typeof request.cookies.authtoken != 'undefined'
                request.headers.authorization = request.cookies.authtoken
            #passport.authenticate('basic', {session:false, failureRedirect: '/user/login' })(request, response, next)
            passport.authenticate('basic', {session:false, failureRedirect: '/api/user/login' })(request, response, next)

    is_whitelisted: (path) ->
        for allowed_path in whitelisted_paths
            if typeof allowed_path is 'object'
                return true if path.match(allowed_path)?
            return true if path is allowed_path
        false

exports = module.exports = Authentication
