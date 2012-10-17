crypto = require('crypto')
fs = require('fs')
path = require('path')
http = require('http')
config = require('./config')()

#private_key = fs.readFileSync(path.join(__dirname, 'certificates', config.env, 'privatekey.pem'))
#certificate = fs.readFileSync(path.join(__dirname, 'certificates', config.env, 'certificate.pem'))

private_key = fs.readFileSync(path.join(__dirname, 'certificates', config.env, 'star_yacketyapp_com.key'))
certificate = fs.readFileSync(path.join(__dirname, 'certificates', config.env, 'sslchain.crt'))


credentials = 
    key: private_key
    cert: certificate

express = require('express')


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

configure_server_for = (application) ->
    application.configure ->
        application.use allowCrossDomain

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
configure_server_for https_app

app.use express.vhost(config.site.host, require('./subdomains/default').app(socket_io))
https_app.use express.vhost(config.api.host, require('./subdomains/api').app(socket_io))
https_app.use express.vhost(config.site.host, require('./subdomains/default').app(socket_io))

app.listen config.site.port, ->
    logger.info "Application Started. Listening on port #{app.address().port}", {mode: app.settings.env}

https_app.listen config.api.port, ->
    logger.info "API Application Started. Listening on port #{https_app.address().port}", {mode: https_app.settings.env}
