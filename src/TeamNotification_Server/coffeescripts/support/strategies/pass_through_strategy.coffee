Q = require('q')

strategy = (value) ->
    Q.fcall () ->
        value

module.exports = strategy
