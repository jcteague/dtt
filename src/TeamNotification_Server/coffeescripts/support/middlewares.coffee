
module.exports =
    socket_io: (io) ->
        return (req, res, next) -> 
            req.socket_io = io
            next()
            
    valid_user: (req,res,next) ->
        user_id = parseInt(req.param('id'), 10)
        if( typeof(user_id)!='undefined' && req.user.id = user_id)
            return next()
        req.redirect('#/user/login')
