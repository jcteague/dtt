express = require('express')
config = require('./../../config')()

Authentication = require('./../../support/authentication')
auth = new Authentication()

app = module.exports = express.createServer()

logger = require('./../../support/logging/logger')
winston = require('winston')
express_winston = require('express-winston')

app.configure ->
    logger.info 'Starting the default module'

    app.set('views', __dirname + '/../../views')
    app.set('view engine', 'jade')
    app.use(express.cookieParser())
    app.use(express.bodyParser())
    app.use(express.methodOverride())

    app.use(auth.initializeAuth())

    app.use(express.static(__dirname + '/../../public'))

    app.use(app.router)

    log_errors = (err, req, res, next) ->
        logger.error 'Error:', err.stack || err.message
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

app.all '*', auth.authenticate

app.configure('development', ->
    app.use(express.errorHandler({ dumpExceptions: true, showStack: true }))
)

app.configure('test', ->
    app.use(express.errorHandler({ dumpExceptions: true, showStack: true }))
)

app.configure('production', ->
    app.use(express.errorHandler())
)

module.exports =
    app: (socket_io) ->
        require('./routes')(app, socket_io)
        app
