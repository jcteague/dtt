
/**
 * Module dependencies.
 */

var express = require('express');

var app = module.exports = express.createServer();
require('./routes')(app);
require('./helper')(app);


/**
 * Mock Database
 */
var apiKeys = ['foo', 'bar', 'baz'];

var error = function(status, msg) {
    var err = new Error(msg);
    err.status = status;
    return err;
}

app.configure(function(){
    app.set('views', __dirname + '/views');
    app.set('view engine', 'jade');
    app.use(express.bodyParser());
    app.use(express.methodOverride());

    app.use('/', function(req, res, next){
        var key = req.param('api-key');
        if (!key) return next(error(400, 'api key required'));
        if (!~apiKeys.indexOf(key)) return next(error(401, 'invalid api key'));
        req.key = key;
        next();
    });
    app.use(app.router);
    app.use(express.static(__dirname + '/public'));
});

app.configure('development', function(){
    app.use(express.errorHandler({ dumpExceptions: true, showStack: true }));
});

app.configure('production', function(){
    app.use(express.errorHandler());
});

app.listen(3000, function(){
    console.log("Express server listening on port %d in %s mode", app.address().port, app.settings.env);
});
