crypto = require('crypto')
fs = require('fs')
http = require('http')

private_key = fs.readFileSync('certificates/privatekey.pem')
certificate = fs.readFileSync('certificates/certificate.pem')

credentials = 
    key: private_key
    cert: certificate

express = require('express')

config = require('./config')()

app = module.exports = express.createServer()
https_app = module.exports = express.createServer(credentials)

logger = require('./support/logging/logger')

socket_io = require('socket.io').listen(https_app, log: false)

process.on 'error', (err) ->
    logger.error 'System error', {error: err}

process.on 'SIGTERM', (err) ->
    logger.info 'SIGTERM event'
    process.exit(0)

process.on 'uncaughtException', (err) ->
    logger.error 'UNCAUGHT EXCEPTION', {error: err}
    app.close()
    process.exit(1)

allowCrossDomain = (req, res, next) ->
    res.header('Access-Control-Allow-Origin', '*')
    res.header('Access-Control-Allow-Methods', 'GET,PUT,POST,DELETE')
    res.header('Access-Control-Allow-Credentials', true)
    res.header('Access-Control-Allow-Headers', 'Authorization, Content-Type')

    if 'OPTIONS' is req.method
        res.send 200
    else
        next()

app.configure ->
    app.use allowCrossDomain

https_app.configure ->
    https_app.use allowCrossDomain

app.configure('development', ->
    app.use(express.errorHandler({ dumpExceptions: true, showStack: true }))
)

app.configure('test', ->
    app.use(express.errorHandler({ dumpExceptions: true, showStack: true }))
)

app.configure('production', ->
    socket_io.set 'log level', 1
    app.use(express.errorHandler())
)

https_app.configure('development', ->
    https_app.use(express.errorHandler({ dumpExceptions: true, showStack: true }))
)

https_app.configure('test', ->
    https_app.use(express.errorHandler({ dumpExceptions: true, showStack: true }))
)

https_app.configure('production', ->
    socket_io.set 'log level', 1
    https_app.use(express.errorHandler())
)

https_app.use express.vhost(config.api.host, require('./subdomains/api').app(socket_io))
app.use express.vhost(config.site.host, require('./subdomains/default').app(socket_io))

app.listen config.site.port, ->
    logger.info "Application Started. Listening on port #{app.address().port}", {mode: app.settings.env}

https_app.listen config.api.port, ->
    logger.info "API Application Started. Listening on port #{https_app.address().port}", {mode: https_app.settings.env}

