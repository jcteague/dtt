###
Module dependencies.
###

express = require('express')
config = require('./config')()
Authentication = require('./support/authentication')
auth = new Authentication()

app = module.exports = express.createServer()
require('./helper')(app)
io = require('socket.io').listen(app)

logger = require('./support/logging/logger')
winston = require('winston')
express_winston = require('express-winston')

###
  Mock Database
###

apiKeys = ['foo', 'bar', 'baz']

error = (status, msg) ->
    err = new Error(msg)
    err.status = status
    return err

process.on 'error', (err) ->
    logger.error 'System error', {error: err}

process.on 'SIGTERM', (err) ->
    logger.info 'SIGTERM event'
    process.exit(0)

process.on 'uncaughtException', (err) ->
    logger.error 'UNCAUGHT EXCEPTION', {error: err}
    app.close()
    process.exit(1)

app.configure(->
    logger.info 'Configuring Application'

    app.set('views', __dirname + '/views')
    app.set('view engine', 'jade')
    app.use(express.cookieParser())
    app.use(express.bodyParser())
    app.use(express.methodOverride())

    app.use('/', (req, res, next) ->
        #disable
        next()
        return
        #disable

        key = req.param('api-key')
        return next(error(400, 'api key required')) if (!key)
        return next(error(401, 'invalid api key')) if (!~apiKeys.indexOf(key))
        req.key = key
        next()
    )

    app.use(express.static(__dirname + '/public'))

    app.use(auth.initializeAuth())

    app.use(app.router)

    log_errors = (err, req, res, next) ->
        logger.error 'Error:', {error: err}
        next err

    rendered_error = (err, req, res, next) ->
        res.status 500
        res.render 'error.jade'

    app.use log_errors

    app.use rendered_error

    app.use(express.logger())

    app.use(express_winston.errorLogger({
        transports: [
            new winston.transports.File({
                filename: config.log.path
            })
        ]
    }))
)

# Apply authentication for all routes
app.all '*', auth.authenticate


# This must live here after authentication has been initialized
require('./routes')(app, io)

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

app.listen(config.site.port, ->
    logger.info "Application Started. Listening on port #{app.address().port}", {mode: app.settings.env}
)
