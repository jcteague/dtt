can_map = (event_obj) ->
  event_obj.pull_request?

map = (event_obj) ->
    message_date = new Date()
    return {
        user:event_obj.pull_request.user.login,
        event_type:'pull_request',
        repository_name:event_obj.repository.name,
        repository_url: event_obj.repository.url
        content:'',
        message: "Github notification! User, #{event_obj.pull_request.user.login}, just did a pull request on repository: #{event_obj.repository.name} {0} - {1}",
        notification:1,
        source:'Github notification'
        url: event_obj.pull_request.diff_url
        date: message_date
        stamp: message_date.getTime()
    }


module.exports =
    can_map: can_map
    map: map
