
module.exports =
    socket_io: (io) ->
        return (req, res, next) -> 
            req.socket_io = io
            next()
            
    valid_user: () ->
        return (req, res, next) ->
            #user_id = req.param('id').toString()
            #if( typeof(user_id)!='undefined' && req.user.id.toString() == user_id)
            next()
            #req.redirect('#/user/login')
