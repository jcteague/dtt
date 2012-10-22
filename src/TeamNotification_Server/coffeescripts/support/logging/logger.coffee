config = require('../../config')()
logentries = require('node-logentries')
winston = require('winston')

logentries_log = logentries.logger { token: config.log.token }
logentries_log.level 'info'

log_message = (level, args...) ->
    msg = []
    for val in args
        if typeof val is 'object'
            msg.push JSON.stringify(val)
        else
            msg.push val

    #console.log 'To save...'
    #console.log msg.join(', ')
    #logentries_log[level](msg.join(', '))

module.exports =
    info: (args...) ->
        log_message('info', args)

    error: (args...) ->
        log_message('err', args)

    warn: (args...) ->
        log_message('warning', args)

    critical: (args...) ->
        log_message('crit', args)
