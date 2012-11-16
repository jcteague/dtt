crypto = require('crypto')
fs = require('fs')
path = require('path')
http = require('http')
config = require('./config')()

express = require('express')

app = module.exports = express.createServer()
logger = require('./support/logging/logger')
socket_io = require('socket.io').listen(app, log: false)

process.on 'error', (err) ->
    logger.error 'System error', {error: err}

process.on 'SIGTERM', (err) ->
    logger.info 'SIGTERM event'
    process.exit(0)

process.on 'uncaughtException', (err) ->
    logger.critical('UNCAUGHT EXCEPTION')
    logger.critical("[Inside 'uncaughtException' event] " + err.stack || err.message)
    app.close()
    process.exit(1)

configure_server_for = (application) ->
	application.configure ->
	    application.use(express.cookieParser())
	    application.use(express.session( secret:'somthing too secret to be true ;)' ))

    application.configure('development', ->
        application.use(express.errorHandler({ dumpExceptions: true, showStack: true }))
    )

    application.configure('test', ->
        application.use(express.errorHandler({ dumpExceptions: true, showStack: true }))
    )

    application.configure('production', ->
        socket_io.set 'log level', 1
        application.use(express.errorHandler())
    )

configure_server_for app

app.use express.vhost(config.site.host, require('./subdomains/default').app(socket_io))

app.listen config.site.port, ->
    logger.info "HTTP Server Started. Listening on port #{app.address().port}", {mode: app.settings.env}
