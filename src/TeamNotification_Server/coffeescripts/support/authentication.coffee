_ = require('underscore')
passport = require('passport')
sha256 = require('node_hash').sha256
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
            if users[0].password != sha256(password)
                return done(null, false)
            user = users[0]
            done(null, id: user.id, email: user.email, name: user.name)

	authenticate: (request,response) ->
		passport.authenticate('basic', {session:false})

exports = module.exports = Authentication
