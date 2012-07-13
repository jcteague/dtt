
module.exports =
    socket_io: (io) ->
        return (req, res, next) -> 
            req.socket_io = io
            next()
