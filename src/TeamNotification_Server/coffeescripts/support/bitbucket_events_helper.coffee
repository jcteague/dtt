
get_oauth_client = require('./oauth_client_provider').bitbucket_oauth_client
config = require("../config")()
promised_https = require('./http/promised_http_requester')
Q = require('q')
set_bitbucket_repository_events = (repositories_uris, room_key, oauth_token, oauth_token_secret) ->
    post_fields =
        type:"POST"
        URL:encodeURI("#{config.site.surl}/bitbucket/events/#{room_key}")

    post_data = JSON.stringify(post_fields)
    repositories_hooks_request = (build_promise_for(repository_uri, oauth_token, oauth_token_secret, post_data) for repository_uri in repositories_uris)
    Q.all repositories_hooks_request

build_promise_for = (repository_url, oauth_token, oauth_token_secret, post_data) ->
    deferred = Q.defer()
    oa = get_oauth_client()
    console.log oauth_token, oauth_token_secret
    console.log "https://api.bitbucket.org#{repository_url}/services/"
    post_service_callback = (data)->
        console.log "SeNT"
        console.log data
        deferred.resolve()
    
    console.log "https://api.bitbucket.org#{repository_url}/services/0?type=POST"
    console.log "oauth_token"
    console.log oauth_token
    console.log "oauth_token_secret"
    console.log oauth_token_secret
    #oa.getProtectedResource "https://api.bitbucket.org#{repository_url}/services/0?type=POST", "POST", oauth_token, oauth_token_secret, post_service_callback
    oa.post "https://api.bitbucket.org#{repository_url}/services/0?type=POST", oauth_token, oauth_token_secret, post_data,"application/x-www-form-urlencoded", (data)->
        console.log "SeNT"
        console.log data
        deferred.resolve()
    deferred.promise

module.exports =
    set_bitbucket_repository_events: set_bitbucket_repository_events

