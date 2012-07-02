start = ->
    return (callback) ->
        process.env.NODE_ENV = 'test'
        require('../../app')
        callback(null, null)

module.exports = 
    start: start
