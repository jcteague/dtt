passport = require 'passport'
BasicStrategy = require('passport-http').BasicStrategy
repository = require './repository'

class Authentication
	constructor: ->
		passport.use(new BasicStrategy({},@findByUserName))

	initializeAuth: ->
		passport.initialize()

	findByUserName: (username,password,done)->
		console.log "looking for #{username}:#{password}"
		return done(null, {username:'john'})

	authenticate: (request,response) ->
		passport.authenticate('basic', {session:false})

exports = module.exports = Authentication
