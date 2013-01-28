
get_oauth_client = require('./oauth_client_provider').bitbucket_oauth_client
config = require("../config")()
promised_https = require('./http/promised_http_requester')
Q = require('q')
set_bitbucket_repository_events = (repositories, owner, room_key, oauth_token, oauth_token_secret) ->
    post_fields =
        type: "POST"
        URL:"#{config.site.surl}/bitbucket/events/#{room_key}"

    post_data = JSON.stringify(post_fields)
    repositories_hooks_request = (build_promise_for(repository, owner, oauth_token, oauth_token_secret, post_data) for repository in repositories)
    Q.all repositories_hooks_request

build_promise_for = (repository, owner, oauth_token, oauth_token_secret, post_data) ->
    deferred = Q.defer()
    oa = get_oauth_client()
    oa.post "https://api.bitbucket.org/1.0/#{owner}/#{repository}/services/", oauth_token, oauth_token_secret, post_data, "JSON", ()->
        deferred.resolve()
    deferred.promise

module.exports =
    set_bitbucket_repository_events: set_bitbucket_repository_events

