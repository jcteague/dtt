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

	findByUserName: (username,password,done)->
		console.log "looking for #{username}:#{password}"
		return done(null, {id: 1, username:'john'})

	authenticate: (request,response) ->
		passport.authenticate('basic', {session:false})

exports = module.exports = Authentication
