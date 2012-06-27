###
Module dependencies.
###

express = require('express')

app = module.exports = express.createServer()
require('./routes')(app)
require('./helper')(app)


###
  Mock Database
###

apiKeys = ['foo', 'bar', 'baz']

error = (status, msg) -> 
    err = new Error(msg)
    err.status = status
    return err

app.configure(->
    app.set('views', __dirname + '/views')
    app.set('view engine', 'jade')
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
    app.use(app.router)
    app.use(express.static(__dirname + '/public'))
)

app.configure('development', ->
    app.use(express.errorHandler({ dumpExceptions: true, showStack: true }))
)

app.configure('test', ->
    app.use(express.errorHandler({ dumpExceptions: true, showStack: true }))
)

app.configure('production', ->
    app.use(express.errorHandler())
)

app.listen(3000, ->
    console.log("Express server listening on port %d in %s mode", app.address().port, app.settings.env)
)
