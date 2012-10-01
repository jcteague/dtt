Repository = require('../repository')
https = require('https')
GitHub = require('../../config')()
Q = require('q')

get_user = (token) ->
    options =
        host: 'api.github.com'
        path: "/user?access_token=#{token}"
        port: 443
        method: 'GET'

    deferred = Q.defer()
    req = https.request options, (res) ->
        res.on 'data', (user) ->
            console.log user
            deferred.resolve(user)
    deferred.promise

strategy = (parameters) ->
    user = get_user(parameters.access_token).then (u) ->
        {access_token:parameters.access_token, user: user.login}
    #rooms = get_rooms(user.login)

module.exports = strategy
