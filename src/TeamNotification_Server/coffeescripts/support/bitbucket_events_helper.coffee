
get_oauth_client = require('./oauth_client_provider').bitbucket_oauth_client
config = require("../config")()
promised_https = require('./http/promised_http_requester')
Q = require('q')
set_bitbucket_repository_events = (repositories_uris, room_key, oauth_token, oauth_token_secret) ->
    room_url = "#{config.site.surl}/bitbucket/events/#{room_key}"
    repositories_hooks_request = (build_promise_for(repository_uri, oauth_token, oauth_token_secret, room_url) for repository_uri in repositories_uris)
    Q.all repositories_hooks_request

build_promise_for = (repository_url, oauth_token, oauth_token_secret, room_url) ->
    deferred = Q.defer()
    oa = get_oauth_client()
    oa.post "https://api.bitbucket.org#{repository_url}/services", oauth_token, oauth_token_secret, {type:"POST", URL:room_url }, (error, data)->
        console.log "SeNT"
        console.log error
        console.log data
        deferred.resolve()
    deferred.promise

module.exports =
    set_bitbucket_repository_events: set_bitbucket_repository_events

