config = require("../config")()
https = require("https")
querystring = require("querystring")
#POST /repos/:owner/:repo/hooks

events = [ 'push', 'issues', 'issue_comment', 'commit_comment', 'pull_request', 'fork']


set_github_repository_events = (repositories, owner, room_key, access_token) ->
    post_fields = 
        name : "web"
        config:
            url:"#{config.site.url}/github/#{room_key}"
        events: events
    post_data = querystring.stringify( post_fields)

    for repository in repositories
        options =
            host: "api.github.com"
            path: "/repos/#{owner}/#{repository}/hooks?access_token=#{access_token}"
            port: 443
            method: 'POST'
            headers:
                'Accept': 'application/json'
                'Content-Type': 'application/x-www-form-urlencoded'
                'Content-Length': post_data.length
                
        console.log options.path

        post_req = https.request options, (post_res) ->        
            post_res.setEncoding('utf8')
            post_res.on 'data', (chunk) ->
                #data = JSON.parse(chunk)
                console.log chunk
                #res.send({success:true, messages:[data.url]})
            post_res.on 'error', (e) -> 
                console.log("Got error: " + e.message)
                #res.send({success:false, messages:["There was an error setting up fthe webhook"]})
        post_req.end(post_data)
    return {succes:true}


module.exports =
    set_github_repository_events : set_github_repository_events
 
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
