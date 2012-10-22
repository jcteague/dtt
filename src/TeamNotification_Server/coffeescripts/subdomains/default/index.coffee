express = require('express')
CORS = require('connect-xcors')
cors_options = {}

config = require('./../../config')()

Authentication = require('./../../support/authentication')
auth = new Authentication()

app = module.exports = express.createServer(CORS(cors_options))

logger = require('./../../support/logging/logger')
winston = require('winston')
express_winston = require('express-winston')

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
    logger.info 'Starting the default module'

    app.set('views', __dirname + '/../../views')
    app.set('view engine', 'jade')
    app.use(express.cookieParser())
    app.use(express.bodyParser())
    app.use(express.methodOverride())

    app.use(express.static(__dirname + '/../../public'))

    app.use allowCrossDomain
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

app.all '*', (req, res, next) ->
    logger.info 'A test log value'
    unless req.secure
        res.redirect "#{config.site.surl}#{req.url}"
    next()

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
