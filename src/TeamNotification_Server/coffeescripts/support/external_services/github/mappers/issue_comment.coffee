can_map = (event_obj) ->
    event_obj.issue? && event_obj.comment?

map = (event_obj) ->
    message_date = new Date()
    return {
        user:event_obj.comment.user.login,
        event_type:'issue_comment',
        repository_name:event_obj.repository.name,
        repository_url:event_obj.repository.html_url ? event_obj.repository.url,
        content:event_obj.issue.body,
        message: "Github notification! User, #{event_obj.comment.user.login}, just commented on issue '#{event_obj.issue.title}' on repository: #{event_obj.repository.name} {0} - {1}",
        notification:1,
        source:'Github notification',
        url: event_obj.issue.html_url ? event_obj.issue.url,  
        date: message_date,
        stamp: message_date.getTime()
    }

module.exports =
    can_map: can_map
    map: map

