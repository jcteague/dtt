app_server = {}
start = ->
    return (callback) ->
        process.env.NODE_ENV = 'test'
        app_server = require('../../app')
        callback(null, null)

stop = ->
    return (callback) ->
        app_server.close()
        callback(null, null)

module.exports = 
    start: start
    stop: stop
