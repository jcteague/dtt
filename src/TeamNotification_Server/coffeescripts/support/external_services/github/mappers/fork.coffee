can_map = (event_obj) ->
    event_obj.forkee?

map = (event_obj) ->
    message_date = new Date()
    return {
        user:event_obj.sender.login,
        event_type:'fork',
        repository_name:event_obj.repository.name,
        repository_url:event_obj.repository.html_url,
        content:'',
        message: "Github notification! User, #{event_obj.sender.login}, just did a fork on repository: #{event_obj.repository.name} {0} - {1}",
        notification:1,
        source:'Github notification'
        url: event_obj.forkee.html_url
        date: message_date
        stamp: message_date.getTime()
    }

module.exports =
    can_map: can_map
    map: map
