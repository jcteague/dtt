passport = require 'passport'
BasicStrategy = require('passport-http').BasicStrategy
Repository = require './repository'

class Authentication
	constructor: ->
        @repository = new Repository('User')
        @basic_strategy = new BasicStrategy({},@findByUserName)
        passport.use(@basic_strategy)

	initializeAuth: ->
		passport.initialize()

    findByUserName: (username,password,done) ->
        console.log "looking for #{username}:#{password}"
        @repository.find(email: username).then (users) ->
            #return done(null, {id: 1, username:'john'})
            done(null, false) unless users? 

	authenticate: (request,response) ->
		passport.authenticate('basic', {session:false})

exports = module.exports = Authentication
