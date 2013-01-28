return
config = require("../config")()
promised_https = require('./http/promised_http_requester')
Q = require('q')
set_bitbucket_repository_events = (repositories, owner, room_key, access_token) ->
    return
    post_fields =
        type: "POST"
        URL:"#{config.site.surl}/bitbucket/events/#{room_key}"

    post_data = JSON.stringify(post_fields)
    repositories_hooks_request = (build_promise_for(repository, owner, access_token, post_data) for repository in repositories)
    Q.all repositories_hooks_request

build_promise_for = (repository, owner, access_token, post_data) ->
    options =
        host: "api.bitbucket.org"
        path: "/#{owner}/#{repository}/services?access_token=#{access_token}"
        port: 443
        method: 'POST'
        headers:
            'Accept': 'application/json'
            'Content-Type': "application/x-www-form-urlencoded"
            'Content-Length': post_data.length
    promised_https.request(post_data, options)

module.exports =
    set_bitbucket_repository_events: set_bitbucket_repository_events

