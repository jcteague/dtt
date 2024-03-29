passport = require('passport')
OAuth2Strategy = require('passport-oauth').OAuth2Strategy

globals = require('./globals')

class OAuthAuthentication

    constructor: (oauth2_strategy = null) ->
        strategy = if oauth2_strategy? then oauth2_strategy else @get_strategy()
        passport.use(strategy)

    get_strategy: ->
        options =
            authorizationURL: "http://#{globals.site.url}/oauth2/authorize"
            tokenURL: "http://#{globals.site.url}/oauth2/token"
            clientID: globals.site.client_ID
            clientSecret: globals.site.client_secret
            callbackURL: "http://#{globals.site.url}/auth/redirect"
        new OAuth2Strategy(options, @verify)

    verify: (accessToken, refreshToken, profile, done) ->


    initializeAuth: ->
        passport.initialize()

    authenticate: ->
        passport.authenticate('oauth2', {})

module.exports = OAuthAuthentication
