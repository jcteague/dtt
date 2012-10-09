can_map = (event_obj) ->
    event_obj.comment?

map = (event_obj) ->
    return {
        user:event_obj.comment.user.login,
        event_type:'comment',
        repository_name:event_obj.repository.name,
        repository_url:event_obj.repository.url,
        content:event_obj.comment.body,
        message:'',
        notification:1,
        source:'Github notification'
        url: event_obj.comment.html_url
    }


module.exports =
    can_map: can_map
    map: map

