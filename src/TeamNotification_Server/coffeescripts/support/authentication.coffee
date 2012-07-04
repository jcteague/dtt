_ = require('underscore')
passport = require('passport')
BasicStrategy = require('passport-http').BasicStrategy
Repository = require './repository'

class Authentication

	constructor: (basic_strategy = null) ->
        _.bindAll @
        @repository = new Repository('User')
        @basic_strategy = if basic_strategy? then basic_strategy else new BasicStrategy({},@findByUserName)
        passport.use(@basic_strategy)

    initializeAuth: ->
        passport.initialize()

    findByUserName: (username,password,done) ->
        @repository.find(email: username).then (users) ->
            if !users? or !users[0]?
                return done(null, false)
            if users[0].password != password
                return done(null, false)
            done(null, users[0])

	authenticate: (request,response) ->
		passport.authenticate('basic', {session:false})

exports = module.exports = Authentication
