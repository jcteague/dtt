can_map = (event_obj) ->
    event_obj.comment?

map = (event_obj) ->
    message_date = new Date()
    return {
        user:event_obj.comment.user.login,
        event_type:'comment',
        repository_name:event_obj.repository.name,
        repository_url:event_obj.repository.html_url ? event_obj.repository.url,
        content:event_obj.comment.body,
        message: "Github notification! User, #{event_obj.comment.user.login}, just did a comment on repository: #{event_obj.repository.name} {0} - {1}",
        notification:1,
        source:'Github notification',
        url: event_obj.comment.html_url ? event_obj.comment.url,  
        date: message_date,
        stamp: message_date.getTime()
    }

module.exports =
    can_map: can_map
    map: map

