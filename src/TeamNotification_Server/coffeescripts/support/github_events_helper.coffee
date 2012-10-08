config = require("../config")()
promised_https = require('./http/promised_http_requester')
Q = require('q')

events = [ 'push', 'issues', 'issue_comment', 'commit_comment', 'pull_request', 'fork']

set_github_repository_events = (repositories, owner, room_key, access_token) ->
    post_fields =
        name : "web"
        events: events
        config:
            content_type: "json"
            url:"#{config.site.url}/github/events/#{room_key}"

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
    if( event_obj.pusher? )
        return {
            user:event_obj.pusher.name,
            event_type:'push',
            repository_name:event_obj.repository.name,
            content:'',
            message:'',
            notification:1,
            source:'Github notification'
        }

    if( event_obj.comment? )
        return {
            user:event_obj.comment.user.login,
            event_type:'comment',
            repository_name:event_obj.repository.name,
            content:event_obj.comment.body,
            message:'',
            notification:1,
            source:'Github notification'
        }

    if( event_obj.forkee? )
        return {
            user:event_obj.sender.login,
            event_type:'fork',
            repository_name:event_obj.repository.name,
            content:'',
            message:'',
            notification:1,
            source:'Github notification'
        }

    return null

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

