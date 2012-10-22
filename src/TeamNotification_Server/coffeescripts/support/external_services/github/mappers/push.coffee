can_map = (event_obj) ->
  event_obj.pusher?

map = (event_obj) ->
    message_date = new Date()
    return {
        user:event_obj.pusher.name,
        event_type:'push',
        repository_name:event_obj.repository.name,
        repository_url: event_obj.repository.url ? event_obj.repository.html_url,
        content:'',
        message: "Github notification! User, #{event_obj.pusher.name}, just did a push on repository: #{event_obj.repository.name} {0} - {1}",
        notification:1,
        source:'Github notification',
        url: event_obj.head_commit.url,
        date: message_date,
        stamp: message_date.getTime()
    }


module.exports =
    can_map: can_map
    map: map
