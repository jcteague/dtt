config = require("../config")()
https = require("https")
Q = require('q')

events = [ 'push', 'issues', 'issue_comment', 'commit_comment', 'pull_request', 'fork']

set_github_repository_events = (repositories, owner, room_key, access_token) ->
    defered = Q.defer()
    post_fields = 
        name : "web"
        events: events
        config: 
            content_type: "json"
            url:"#{config.site.url}/github/events/#{room_key}"
    post_data = JSON.stringify(post_fields)
    console.log post_data
    for repository in repositories
        options =
            host: "api.github.com"
            path: "/repos/#{owner}/#{repository}/hooks?access_token=#{access_token}"
            port: 443
            method: 'POST'
            headers:
                'Accept': 'application/json'
                'Content-Type': "application/x-www-form-urlencoded"
                'Content-Length': post_data.length
                
        console.log options.path
        
        post_req = https.request options, (post_res) ->        
            post_res.setEncoding('utf8')
            post_res.on 'data', (chunk) ->
                console.log chunk
                defered.resolve({succcess:true})
            post_res.on 'error', (e) -> 
                console.log("Got error: " + e.message)
        post_req.end(post_data)
    defered.promise

#{ user:'', event_type:'', repository_name:'', content:''}
get_event_message_object = (event_obj) ->
    console.log event_obj
    if( typeof(event_obj.pusher) != 'undefined' )
        return {user:event_obj.pusher.name, event_type:'push', repository_name:event_obj.repository.name, content:''}
        
    if( typeof(event_obj.comment) != 'undefined' )
        return {user:event_obj.user.login, event_type:'comment', repository_name:event_obj.repository.name, content:event_obj.comment.body}
    
    if( typeof(event_obj.forkee) != 'undefined' )
        return {user:event_obj.sender.login, event_type:'fork', repository_name:event_obj.repository.name, content:''}

    return { user:'', event_type:'', repository_name:''}

module.exports =
    set_github_repository_events : set_github_repository_events
    get_event_message_object : get_event_message_object
 
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
