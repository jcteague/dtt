can_map = (event_obj) ->
    event_obj.forkee?

map = (event_obj) ->
    return {
        user:event_obj.sender.login,
        event_type:'fork',
        repository_name:event_obj.repository.name,
        repository_url:event_obj.repository.url,
        content:'',
        message:'',
        notification:1,
        source:'Github notification'
        url: event_obj.forkee.html_url
    }

module.exports =
    can_map: can_map
    map: map
