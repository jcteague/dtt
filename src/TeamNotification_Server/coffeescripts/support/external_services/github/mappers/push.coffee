can_map = (event_obj) ->
  event_obj.pusher?

map = (event_obj) ->
    return {
        user:event_obj.pusher.name,
        event_type:'push',
        repository_name:event_obj.repository.name,
        repository_url: event_obj.repository.url
        content:'',
        message:'',
        notification:1,
        source:'Github notification'
        url: event_obj.head_commit.url
    }


module.exports =
    can_map: can_map
    map: map
