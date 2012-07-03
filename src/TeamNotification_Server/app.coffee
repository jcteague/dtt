###
Module dependencies.
###

express = require('express')


passport = require('passport')
BasicStrategy = require('passport-http').BasicStrategy

passport.serializeUser (user, done) ->
    done(null, 1)

passport.deserializeUser (id, done) ->
    done(err, {username: 'john'})

passport.use(new BasicStrategy({}, (username, password, done) ->
    done(null, {username: 'john'})
))



app = module.exports = express.createServer()
require('./helper')(app)

#Authentication = require('./support/authentication')
#auth = new Authentication()
#


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

    app.use(express.static(__dirname + '/public'))

    #app.use(auth.initializeAuth())
    app.use(passport.initialize())
    
    app.use(app.router)
)

app.get '/auth', passport.authenticate('basic', session: false), (req, res) ->
    console.log req
    res.send 'SUCCESS!!!!!!!!!!!'

require('./routes')(app)

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
