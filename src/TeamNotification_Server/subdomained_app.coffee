express = require('express')

config = require('./config')()

app = module.exports = express.createServer()
logger = require('./support/logging/logger')


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
    #res.header('Access-Control-Allow-Origin', config.allowedDomains)
    res.header('Access-Control-Allow-Origin', '*')
    res.header('Access-Control-Allow-Methods', 'GET,PUT,POST,DELETE')
    res.header('Access-Control-Allow-Credentials', true)
    res.header('Access-Control-Allow-Headers', 'Authorization, Content-Type')
    #res.header('Access-Control-Allow-Headers', 'Content-Type')
    #res.header('Access-Control-Allow-Headers', 'Authorization')

    if 'OPTIONS' is req.method
        res.send 200
    else
        next()

app.configure ->
    app.use allowCrossDomain


app.configure('development', ->
    app.use(express.errorHandler({ dumpExceptions: true, showStack: true }))
)

app.configure('test', ->
    app.use(express.errorHandler({ dumpExceptions: true, showStack: true }))
)

app.configure('production', ->
    io.set 'log level', 1
    app.use(express.errorHandler())
)

app.use express.vhost("api.#{config.site.host}", require('./subdomains/api').app)
app.use express.vhost("#{config.site.host}", require('./subdomains/default').app)
app.listen(config.site.port, ->
    logger.info "Application Started. Listening on port #{app.address().port}", {mode: app.settings.env}
)

