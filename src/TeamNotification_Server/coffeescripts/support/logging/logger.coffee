config = require('../../config')()
logentries = require('node-logentries')
winston = require('winston')
loggr = require("loggr")
log = loggr.logs.get(config.log.logkey, config.log.apikey)
#logentries_log = logentries.logger { token: config.log.token }
#logentries_log.level 'info'

log_message = (level, args) ->
    console.log args
    msg = []
    for val in args
        if typeof val is 'object'
            msg.push JSON.stringify(val)
        else
            msg.push val
    title = msg[0].substring( 0, 45 )
    console.log title
    logMessage = msg.join(', ')
    log.events.createEvent().text(title).data(logMessage).tags(level).source("web service").post()

module.exports =
    info: (args...) ->
        log_message('info', args)

    error: (args...) ->
        log_message('error', args)

    warn: (args...) ->
        log_message('warning', args)

    critical: (args...) ->
        log_message('crit', args)
