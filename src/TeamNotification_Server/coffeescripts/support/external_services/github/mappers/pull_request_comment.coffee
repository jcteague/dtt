can_map = (event_obj) ->
    event_obj.issue? && event_obj.comment? && event_obj.issue.pull_request? && event_obj.issue.pull_request.html_url?

map = (event_obj) ->
    message_date = new Date()
    return {
        user:event_obj.issue.user.login,
        event_type:'pull_request_comment',
        repository_name:event_obj.repository.name,
        repository_url:event_obj.repository.html_url ? event_obj.repository.url,
        content:event_obj.issue.body,
        message: "Github notification! User, #{event_obj.comment.user.login}, just commented on pull request titled '#{event_obj.issue.title}' on repository: #{event_obj.repository.name} {0} - {1}",
        notification:1,
        source:'Github notification',
        url: event_obj.issue.pull_request.html_url ? event_obj.issue.html_url,  
        date: message_date,
        stamp: message_date.getTime()
    }

module.exports =
    can_map: can_map
    map: map
