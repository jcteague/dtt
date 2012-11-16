config = require("../config")()
promised_https = require('./http/promised_http_requester')
Q = require('q')
event_object_mapper = require('./external_services/github/messages_mapper')

events = [ 'push', 'issues', 'issue_comment', 'commit_comment', 'pull_request', 'fork']

set_github_repository_events = (repositories, owner, room_key, access_token) ->
    post_fields =
        name : "web"
        events: events
        config:
            content_type: "json"
            url:"#{config.site.surl}/github/events/#{room_key}"

    post_data = JSON.stringify(post_fields)
    repositories_hooks_request = (build_promise_for(repository, owner, access_token, post_data) for repository in repositories)
    Q.all repositories_hooks_request

build_promise_for = (repository, owner, access_token, post_data) ->
    options =
        host: "api.github.com"
        path: "/repos/#{owner}/#{repository}/hooks?access_token=#{access_token}"
        port: 443
        method: 'POST'
        headers:
            'Accept': 'application/json'
            'Content-Type': "application/x-www-form-urlencoded"
            'Content-Length': post_data.length

    promised_https.request(post_data, options)

get_event_message_object = (event_obj) ->
    event_object_mapper.map event_obj

###
"supported_events": [
  "commit_comment",
  "create",
  "delete",
  "download",
  "follow",
  "fork",
  "fork_apply",
  "gist",
  "gollum",
  "issue_comment",
  "issues",
  "member",
  "public",
  "pull_request",
  "pull_request_review_comment",
  "push",
  "status",
  "team_add",
  "watch"
]
###

module.exports =
    set_github_repository_events: set_github_repository_events
    get_event_message_object: get_event_message_object

