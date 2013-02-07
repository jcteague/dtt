
get_oauth_client = require('./oauth_client_provider').bitbucket_oauth_client
config = require("../config")()
promised_https = require('./http/promised_http_requester')
Q = require('q')
set_bitbucket_repository_events = (repositories_uris, room_key, oauth_token, oauth_token_secret) ->
    #url = encodeURI("#{config.site.surl}/bitbucket/events/#{room_key}")
    #url = encodeURI("#{config.site.surl}/bitbucket/events/#{room_key}")
    #post_data ="URL=#{url}"
    room_url = "#{config.site.surl}/bitbucket/events/#{room_key}"
    #post_data = url #JSON.stringify(post_fields)
    repositories_hooks_request = (build_promise_for(repository_uri, oauth_token, oauth_token_secret, room_url) for repository_uri in repositories_uris)
    Q.all repositories_hooks_request

build_promise_for = (repository_url, oauth_token, oauth_token_secret, room_url) ->
    deferred = Q.defer()
    oa = get_oauth_client()
    console.log oauth_token, oauth_token_secret
    console.log "https://api.bitbucket.org#{repository_url}/services/"
    post_service_callback = (error, data, response) ->
        console.log "SeNT"
        console.log error
        console.log data
        console.log response
        deferred.resolve()
    
    console.log "https://api.bitbucket.org#{repository_url}/services"
    console.log "oauth_token"
    console.log oauth_token
    console.log "oauth_token_secret"
    console.log oauth_token_secret
    console.log 
    #oa._performSecureRequest oauth_token, oauth_token_secret, "POST", "https://api.bitbucket.org#{repository_url}/services", null, post_data, "application/x-www-form-urlencoded", post_service_callback
    #oa.getProtectedResource "https://api.bitbucket.org#{repository_url}/services/?type=POST&#{post_data}", "POST", oauth_token, oauth_token_secret, post_service_callback
    oa.post "https://api.bitbucket.org#{repository_url}/services", oauth_token, oauth_token_secret, {type:"POST", URL:room_url }, (error, data)->
        console.log "SeNT"
        console.log error
        console.log data
        deferred.resolve()
    deferred.promise

module.exports =
    set_bitbucket_repository_events: set_bitbucket_repository_events

